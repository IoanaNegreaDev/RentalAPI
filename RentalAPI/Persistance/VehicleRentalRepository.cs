using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class VehicleRentalRepository: GenericRepository<VehicleRental>, IVehicleRentalRepository
    {
		public VehicleRentalRepository(RentalDbContext context) : base(context)
		{ }

		override public async Task<IEnumerable<VehicleRental>> ListAsync()
			=> await _table
						.Include(item=>item.RentedItem).ThenInclude(item => ((Vehicle)item).Fuel)
						.Include(item => item.RentedItem).ThenInclude(item => item.Category).ThenInclude(item => item.Domain)
						.Include(item=>item.RentalDamages)
						.ToListAsync();
		override public async Task<VehicleRental> FindByIdAsync(int id)
			=> await _table.Where(item => item.Id == id)
						.Include(item => item.RentedItem).ThenInclude(item => ((Vehicle)item).Fuel)
						.Include(item => item.RentedItem).ThenInclude(item => item.Category).ThenInclude(item => item.Domain)
						.Include(item => item.RentalDamages)
					.FirstOrDefaultAsync();
	}
}
