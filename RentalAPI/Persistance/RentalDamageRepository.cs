using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class RentalDamageRepository: GenericRepository<RentalDamage>, IRentalDamageRepository
    {
		public RentalDamageRepository(RentalDbContext context) : base(context)
		{ }

		override public async Task<IEnumerable<RentalDamage>> ListAsync()
			=> await _table.Include(item => item.Damage).ToListAsync();
	}
}
