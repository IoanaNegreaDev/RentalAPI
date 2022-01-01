using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class VehicleContractRepository: GenericRepository<VehicleContract>, IVehicleContractRepository
    {
		public VehicleContractRepository(RentalDbContext context) : base(context)
		{ }

		override public async Task<IEnumerable<VehicleContract>> ListAsync()
			=> await _table.Include(item => item.Rentals).ToListAsync();
	
		override public async Task<VehicleContract> FindByIdAsync(int id)
			=> await _table.Include(item => item.Rentals)
												.Where(item => item.Id == id)
												.FirstOrDefaultAsync();
	}
}
