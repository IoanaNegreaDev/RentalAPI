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

		override public async Task<VehicleRental> FindByIdAsync(int id)
			=> await _table.Include(item => item.RentalDamages)
												.ThenInclude(item => item.Damage)
												.Where(item => item.Id == id).FirstOrDefaultAsync();
	}
}
