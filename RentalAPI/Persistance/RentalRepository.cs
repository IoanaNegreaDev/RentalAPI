using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class RentalRepository: GenericRepository<Rental>, IRentalRepository
	{
		public RentalRepository(RentalDbContext context) : base(context)
		{
		}

		virtual public async Task<Rental> FindByIdAsync(int contractId, int rentalId)
		   => await _table.Where(item => item.Id == rentalId && item.ContractId == contractId)
						.Include(item => item.RentedItem).ThenInclude(item => item.Category).ThenInclude(item => item.Domain)
						.Include(item => item.RentalDamages)
						.FirstOrDefaultAsync();

		virtual public async Task<IEnumerable<Rental>> ListAsync(int contractId)
			=> await _table
					.Where(rental => rental.ContractId == contractId)
					.Include(item => item.RentedItem).ThenInclude(item => item.Category).ThenInclude(item => item.Domain)
					.Include(item => item.RentalDamages)
					.ToListAsync();
	}	
}
