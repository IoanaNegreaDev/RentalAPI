using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
	{
		public RentalRepository(RentalDbContext context) : base(context)
		{ }

		override public async Task<IEnumerable<Rental>> ListAsync()
			=> await _table
						.Include(item => item.RentedItem).ThenInclude(item =>item.Category).ThenInclude(item=>item.Domain)
						.Include(item => item.RentalDamages)
						.ToListAsync();
		override public async Task<Rental> FindByIdAsync(int id)
			=> await _table.Where(item => item.Id == id)
						.Include(item => item.RentedItem).ThenInclude(item => item.Category).ThenInclude(item => item.Domain)
						.Include(item => item.RentalDamages)
						.FirstOrDefaultAsync();
	}	
}
