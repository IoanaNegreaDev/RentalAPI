using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        public Task<User> FindByUserNameAndPasswordAsync(string userName, string password);
    }
}
