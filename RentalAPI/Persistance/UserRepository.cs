using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class UserRepository : GenericRepository<RentalUser>, IUserRepository
    {
        public UserRepository(RentalDbContext context) : base(context)
        { }

        public async Task<RentalUser> FindByIdAsync(string id)
            => await _table.Where(item => item.Id == id).FirstOrDefaultAsync();

        public async Task<RentalUser> GetUserWithTokenAsync(string token)
             => await _table.Include(item => item.RefreshTokens)
                            .Where(user => user.RefreshTokens
                                .Any(refrToken => refrToken.Token == token && refrToken.RentalUserId == user.Id))
                            .FirstOrDefaultAsync();
       
    }
}
