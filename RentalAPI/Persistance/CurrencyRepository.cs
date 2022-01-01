using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class CurrencyRepository:BaseRepository, ICurrencyRepository
    {
		public CurrencyRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<IEnumerable<Currency>> ListAsync()
		{
			return await _context.Currencies.ToListAsync();
		}
		public async Task<Currency> FindByNameAsync(string name)
		{
			return await _context.Currencies.Where(item =>
												item.Name
												.ToLower()
												.Contains(name.ToLower()))
											.FirstOrDefaultAsync();
		}

		public async Task<Currency> GetDefaultAsync()
        {
			return await _context.Currencies.Where(item =>
									item.Default == true)
								.FirstOrDefaultAsync();
		}
	}
}
