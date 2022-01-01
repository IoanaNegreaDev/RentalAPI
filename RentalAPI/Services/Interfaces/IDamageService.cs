using RentalAPI.Models;
using RentalAPI.Services.DbOperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IDamageService
    {
        public Task<IEnumerable<Damage>> ListAsync();
        public Task<DamageOperationResponse> AddAsync(Damage damage);
        public Task<Damage> FindByIdAsync(int id);
    }
}
