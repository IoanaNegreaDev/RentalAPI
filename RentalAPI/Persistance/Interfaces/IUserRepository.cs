using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        public Task<User> FindByUserNameAndPasswordIncludeRefsAsync(string userName, string password);
    }
}
