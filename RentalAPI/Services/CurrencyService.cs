using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class CurrencyService:BaseService<Currency, ICurrencyRepository>, ICurrencyService
    {
        public CurrencyService(ICurrencyRepository repository, IUnitOfWork unitOfWork) 
            : base(repository, unitOfWork)
        {
        }

        public async Task<Currency> FindByNameAsync(string name)
            => await _repository.FindByNameAsync(name);

        public async Task<Currency> GetDefaultAsync()
         => await _repository.GetDefaultAsync();
    }
}
