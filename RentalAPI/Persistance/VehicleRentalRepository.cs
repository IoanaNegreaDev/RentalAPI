using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class VehicleRentalRepository: BaseRepository, IVehicleRentalRepository
    {
		public VehicleRentalRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<IEnumerable<VehicleRental>> ListAsync()
		{
			return await _context.VehicleRentals.ToListAsync();
		}
		public async Task<VehicleRental> FindByIdAsync(int id)
		{
			return await _context.VehicleRentals.Include(item => item.RentalDamages)
												.ThenInclude(item => item.Damage)
												.Where(item => item.Id == id).FirstOrDefaultAsync();
		}
		public async Task AddAsync(VehicleRental Rental)
		{
			await _context.VehicleRentals.AddAsync(Rental);
		}
		public void Update(VehicleRental Rental)
		{
			_context.VehicleRentals.Update(Rental);
		}
	}
}
