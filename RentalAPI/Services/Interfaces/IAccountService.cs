using RentalAPI.Models;
using RentalAPI.Services.Authentication;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<DbOperationResponse<UserWithToken>> RegisterAsync(UserCredentials credentials);
        public Task<DbOperationResponse<UserWithToken>> LoginAsync(UserCredentials credentials);
        public Task<DbOperationResponse<UserWithToken>> LogoutAsync(string refreshToken);
        public Task<DbOperationResponse<UserWithToken>> RefreshTokensAsync(string refreshToken);
    }
}
