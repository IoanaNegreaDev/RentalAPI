using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class PaymentRepository:GenericRepository<Payment>, IPaymentRepository
    {
		public PaymentRepository(RentalDbContext context) : base(context)
		{
		}

		override public async Task<IEnumerable<Payment>> ListAsync()
			=> await _table.Include(item => item.Contract).ThenInclude(item => item.Client)
						   .Include(item => item.Contract).ThenInclude(item => item.Rentals)
						   .Include(item => item.Contract).ThenInclude(item => item.Currency)
						   .ToListAsync();
	}
}
