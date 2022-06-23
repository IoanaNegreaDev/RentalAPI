using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class CurrencyRepository: GenericRepository<Currency>, ICurrencyRepository
	{
		public CurrencyRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<Currency> FindByNameAsync(string name)
			=> await _table.Where(item =>
												item.Name
												.ToLower()
												.Contains(name.ToLower()))
											.FirstOrDefaultAsync();

		public async Task<Currency> GetDefaultAsync()
			=> await _table.Where(item =>
											item.Default == true)
										.FirstOrDefaultAsync();
	}
}
