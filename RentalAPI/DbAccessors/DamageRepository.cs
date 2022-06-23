using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class DamageRepository : GenericRepository<Damage>, IDamageRepository
    {
        public DamageRepository(RentalDbContext context) : base(context)
        {
        }

		virtual public async Task<IEnumerable<Damage>> ListAsync(int contractId, int rentalId)
			=> await _table.Where(damage => damage.OccuredInRentalId == rentalId && damage.Rental.ContractId == contractId)
						   .ToListAsync();
		virtual public async Task<Damage> FindByIdAsync(int contractId, int rentalId, int damageId)
			=> await _table.Where(damage => damage.OccuredInRentalId == rentalId &&
											damage.Rental.ContractId == contractId &&
											damage.Id == damageId).FirstOrDefaultAsync();
	}
}
