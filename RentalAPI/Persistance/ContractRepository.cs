using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class ContractRepository: GenericRepository<Contract>, IContractRepository
    {
		public ContractRepository(RentalDbContext context) : base(context)
		{ }

		override public async Task<IEnumerable<Contract>> ListAsync()
			=> await _table
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentalDamages)
						.ThenInclude(item => item.Damage)
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentedItem)
					.Include(item => item.User)
					.Include(item=>item.Currency)
					.ToListAsync();

		override public async Task<Contract> FindByIdAsync(int id)
			=> await _table
					.Where(item => item.Id == id)
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentalDamages)
						.ThenInclude(item => item.Damage)
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentedItem)
					.Include(item => item.Currency)
				    .FirstOrDefaultAsync();

		public async Task RemoveAsync(Contract item)
		{
			var itemWithRentals = await _table
				.Where(c =>c.Id == item.Id)
				.Include(item => item.Rentals)
				.FirstOrDefaultAsync();
			 _table.Remove(itemWithRentals);
		}
	}
}
