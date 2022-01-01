using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IRentalDamageRepository
    {
        public Task<IEnumerable<RentalDamage>> ListAsync();
        public Task<RentalDamage> FindByIdAsync(int id);
        public Task AddAsync(RentalDamage damage);
    }
}
