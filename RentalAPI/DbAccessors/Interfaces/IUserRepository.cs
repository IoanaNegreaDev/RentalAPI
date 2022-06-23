using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IUserRepository: IGenericRepository<RentalUser>
    {
        public Task<RentalUser> FindByIdAsync(string name);
        public Task<RentalUser> GetUserWithTokenAsync(string token);    
    }
}
