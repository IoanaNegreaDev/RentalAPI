using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Authentication;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly byte[] _secretKey;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IUserRepository _repository;
        private readonly UserManager<RentalUser> _userManager;
        private readonly SignInManager<RentalUser> _userLogin;
        protected readonly string _issuer;
        protected readonly string _audience;

        public AccountService(UserManager<RentalUser> userManager,
                             SignInManager<RentalUser> userLogin,
                                IUserRepository repository,
                                IUnitOfWork unitOfWork,
                                IConfiguration configuration)

        {
            _secretKey = Encoding.ASCII.GetBytes(configuration.GetSection("JWTSettings:SecretKey").Value);
            _issuer = configuration.GetSection("JWTSettings:ValidIssuer").Value;
            _audience = configuration.GetSection("JWTSettings:ValidAudience").Value;
            _unitOfWork = unitOfWork;
            _userLogin = userLogin;
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<DbOperationResponse<IdentityResult>> RegisterAsync(UserCredentials credentials)
        {
            var dbUser = await _userManager.FindByNameAsync(credentials.UserName);
            if (dbUser != null)
                return new DbOperationResponse<IdentityResult>("User already exists.");

            dbUser = new RentalUser()
            {
                UserName = credentials.UserName,
                PhoneNumber = credentials.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(dbUser, credentials.Password);

                if (!result.Succeeded)
                    return new DbOperationResponse<IdentityResult>("Failed to register. " + result.Errors);

                return new DbOperationResponse<IdentityResult>(result);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<IdentityResult>("Failed to add user with token to the database. " + ex.Message);
            }
        }

        public async Task<DbOperationResponse<UserWithToken>> LoginAsync(UserCredentials credentials)
        {
            var user = await ValidateUserAsync(credentials);
            if (user == null)
                return new DbOperationResponse<UserWithToken>("Invalid user namename or password.");

            var refreshTokensResult = await GenerateTokensAsync(user);
            if (!refreshTokensResult.Success)
                return new DbOperationResponse<UserWithToken>("Failed to refresh token for user." + refreshTokensResult.Message);

            await _userLogin.SignInAsync(user, true);

            return new DbOperationResponse<UserWithToken>(refreshTokensResult._entity);
        }

        public async Task<DbOperationResponse<UserWithToken>> LogoutAsync(string refreshToken)
        {
            if (await RevokeRefreshToken(refreshToken))
                return new DbOperationResponse<UserWithToken>("Logged Out");
            await _userLogin.SignOutAsync();
            return new DbOperationResponse<UserWithToken>("Failed to logout.");
        }

        public async Task<DbOperationResponse<UserWithToken>> RefreshTokensAsync(string refreshToken)
        {
            var user = await _repository.GetUserWithTokenAsync(refreshToken);

            // Get existing refresh token if it is valid and revoke it
            var existingRefreshToken = GetValidRefreshToken(refreshToken, user);
            if (existingRefreshToken == null)
                return new DbOperationResponse<UserWithToken>("Failed to get a valid refresh token.");
         
            existingRefreshToken.RevokedOn = DateTime.UtcNow;

            // Generate new tokens
           return await GenerateTokensAsync(user);
        }

        private async Task<RentalUser> ValidateUserAsync(UserCredentials credentials)
        {
            var identityUser = await _userManager.FindByNameAsync(credentials.UserName);
            if (identityUser != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            return null;
        }

        private async Task<DbOperationResponse<UserWithToken>> GenerateTokensAsync(RentalUser user)
        {
            if (user == null)
                return new DbOperationResponse<UserWithToken>("Internal error. Invalid user pointer. ");

            RefreshToken refreshToken = GenerateRefreshToken();
            var accessToken = GenerateAccessToken(user);

            try
            {
                if (user.RefreshTokens == null)
                    user.RefreshTokens = new List<RefreshToken>();

                user.RefreshTokens.Add(refreshToken);
                await _unitOfWork.SaveChangesAsync();

                var userWithToken = new UserWithToken(user);
                userWithToken.RefreshToken = refreshToken.Token;
                userWithToken.AccessToken = accessToken;

                return new DbOperationResponse<UserWithToken>(userWithToken);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<UserWithToken>("Failed to find user and refresh token. " + ex.Message);
            }
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.CreationDate = DateTime.Now;
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }

        private string GenerateAccessToken(RentalUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", Convert.ToString(user.Id)),
                    new Claim("PhoneNumber", user.PhoneNumber),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<bool> RevokeRefreshToken(string token)
        {
            var identityUser = await _repository.GetUserWithTokenAsync(token);

            if (identityUser == null)
            {
                return false;
            }

            var existingToken = identityUser.RefreshTokens.FirstOrDefault(x => x.Token == token);
            existingToken.RevokedOn = DateTime.UtcNow;

            _repository.Update(identityUser);
            return true;
        }

        private RefreshToken GetValidRefreshToken(string token, RentalUser user)
        {
            if (user == null)
                return null;

            var existingToken = user.RefreshTokens.FirstOrDefault(x => x.Token == token);
            return IsRefreshTokenValid(existingToken) ? existingToken : null;
        }

        private bool IsRefreshTokenValid(RefreshToken existingToken)
        {
            // Is token already revoked, then return false
            if (existingToken.RevokedOn != DateTime.MinValue)
            {
                return false;
            }

            // Token already expired, then return false
            if (existingToken.ExpiryDate <= DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}
