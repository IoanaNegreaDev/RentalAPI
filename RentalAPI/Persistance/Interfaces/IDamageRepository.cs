using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IDamageRepository:IGenericRepository<Damage>
    {
        public Task<IEnumerable<Damage>> ListAsync(int contractId, int rentalId);
        public Task<Damage> FindByIdAsync(int contractId, int rentalId, int damageId);
     }
}
