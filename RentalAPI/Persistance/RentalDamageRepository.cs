using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class RentalDamageRepository: BaseRepository, IRentalDamageRepository
    {
		public RentalDamageRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<IEnumerable<RentalDamage>> ListAsync()
		{
			return await _context.RentalDamages.Include(item => item.Damage).ToListAsync();
		}
		public async Task<RentalDamage> FindByIdAsync(int id)
		{
			return await _context.RentalDamages.FindAsync(id);
		}
		public async Task AddAsync(RentalDamage damage)
		{
			await _context.RentalDamages.AddAsync(damage);
		}
	}
}
