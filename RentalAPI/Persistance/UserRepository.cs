using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(RentalDbContext context) : base(context)
        { }

        public async Task<User> FindByUserNameAndPasswordAsync(string userName, string password)
               => await _table
                .Where(item => item.UserName == userName && item.Password == password)
                .FirstOrDefaultAsync();
    }
}
