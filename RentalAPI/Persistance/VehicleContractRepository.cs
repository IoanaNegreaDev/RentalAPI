using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class VehicleContractRepository: BaseRepository, IVehicleContractRepository
    {
		public VehicleContractRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<IEnumerable<VehicleContract>> ListAsync()
		{
			return await _context.VehicleContracts.ToListAsync();
		}
		public async Task<VehicleContract> FindByIdAsync(int id)
		{
			return await _context.VehicleContracts.Include(item => item.Rentals)
												.Where(item => item.Id == id)
												.FirstOrDefaultAsync();

		}
		public async Task AddAsync(VehicleContract contract)
		{
			await _context.VehicleContracts.AddAsync(contract);
		}
		public void Update(VehicleContract contract)
		{
			_context.VehicleContracts.Update(contract);
		}
	}
}
