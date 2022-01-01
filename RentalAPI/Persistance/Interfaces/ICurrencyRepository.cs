using RentalAPI.Models;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface ICurrencyRepository: IGenericRepository<Currency>
    { 
        public Task<Currency> FindByNameAsync(string name);
        public Task<Currency> GetDefaultAsync();
    }
}
