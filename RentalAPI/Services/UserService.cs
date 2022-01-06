using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
    public class UserService: BaseService<User, IUserRepository>, IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repository, IUnitOfWork unitOfWork, IConfiguration configuration)
          : base(repository, unitOfWork)
        {
            _configuration = configuration;
        }

        public async Task<User> FindByUserNameAndPasswordAsync(string userName, string password)
           => await _repository.FindByUserNameAndPasswordAsync(userName, password);

        public async Task<DbOperationResponse<UserWithToken>> AddUserWithTokenAsync(User user)
        {
            var dbUser = await _repository.FindByUserNameAndPasswordAsync(user.UserName, user.Password);
            if (dbUser != null)
                return new DbOperationResponse<UserWithToken>("User already exists.");

            RefreshToken refreshToken = GenerateRefreshToken();
            user.RefreshTokens = new List<RefreshToken>() { refreshToken };

            try
            {
                await _repository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var accessToken = GenerateAccessToken(user.Id);

                var userWithToken = new UserWithToken(user)
                {
                    RefreshToken = refreshToken.Token,
                    AccessToken = accessToken,
                };

                return new DbOperationResponse<UserWithToken>(userWithToken);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<UserWithToken>("Failed to add user with token to the database. " + ex.Message);
            }
        }

        public async Task<DbOperationResponse<UserWithToken>> FindUserAndRefreshTokenAsync(User user)
        {
            var dbUser = await _repository.FindByUserNameAndPasswordAsync(user.UserName, user.Password);
            if (dbUser == null)
                return new DbOperationResponse<UserWithToken>("User not found in database.");

            RefreshToken refreshToken = GenerateRefreshToken();
            try
            {
                user.RefreshTokens.Add(refreshToken);
                await _unitOfWork.SaveChangesAsync();

                UserWithToken userWithToken = new UserWithToken(user);
                userWithToken.RefreshToken = refreshToken.Token;
                userWithToken.AccessToken = GenerateAccessToken(user.Id);

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
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }

        private string GenerateAccessToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTSetiings:SecretKey").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(userId))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = "me",
                Audience = "world",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
