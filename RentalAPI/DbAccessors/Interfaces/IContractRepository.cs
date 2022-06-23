using RentalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IContractRepository:IGenericRepository<Contract>
    {
        public Task<Contract> FindByIdAsync(string userId, int id);
        public Task<IEnumerable<Contract>> ListAsync(string userId);
        public Task RemoveAsync(Contract item);
    }
}
