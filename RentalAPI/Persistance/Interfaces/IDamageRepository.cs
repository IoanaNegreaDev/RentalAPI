using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IDamageRepository
    {
        public Task<IEnumerable<Damage>> ListAsync();
        public Task<Damage> FindByIdAsync(int id);
        public Task AddAsync(Damage damage);
    }
}
