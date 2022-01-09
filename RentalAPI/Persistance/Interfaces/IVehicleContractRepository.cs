using RentalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IContractRepository:IGenericRepository<Contract>
    {
        public Task RemoveAsync(Contract item);
    }
}
