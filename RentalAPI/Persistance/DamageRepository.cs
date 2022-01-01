using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class DamageRepository: BaseRepository, IDamageRepository
    {
		public DamageRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<IEnumerable<Damage>> ListAsync()
		{
			return await _context.Damages.ToListAsync();
		}
		public async Task<Damage> FindByIdAsync(int id)
		{
			return await _context.Damages.FindAsync(id);
		}
		public async Task AddAsync(Damage damage)
		{
			await _context.Damages.AddAsync(damage);
		}
	}
}
