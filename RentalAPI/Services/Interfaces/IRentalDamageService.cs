using RentalAPI.Models;
using RentalAPI.Services.DbOperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IRentalDamageService
    {
        public Task<IEnumerable<RentalDamage>> ListAsync();
        public Task<RentalDamageOperationResponse> AddAsync(RentalDamage rentalDamage);
        public Task<DamageOperationResponse> AddAsync(Damage damage);
        public Task<RentalDamage> FindByIdAsync(int id);

    }
}
