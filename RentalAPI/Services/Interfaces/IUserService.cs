using RentalAPI.Models;
using RentalAPI.Services.Authentication;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        public Task<User> FindByUserNameAndPasswordAsync(string userName, string password);
        public Task<DbOperationResponse<UserWithToken>> AddUserWithTokenAsync(User user);
        public Task<DbOperationResponse<UserWithToken>> RefreshUserTokenAsync(User user);
        
    }
}
