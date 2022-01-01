using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class PaymentRepository:BaseRepository, IPaymentRepository
    {
		public PaymentRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<IEnumerable<Payment>> ListAsync()
		{
			return await _context.Payments.Include(item => item.Contract).ThenInclude(item => item.Client)
										  .Include(item => item.Contract).ThenInclude(item => item.Rentals)
										  .Include(item => item.Currency)
										  .ToListAsync();
		}
		public async Task<Payment> FindByIdAsync(int id)
		{
			return await _context.Payments.FindAsync(id);
		}
		public async Task AddAsync(Payment payment)
		{
			await _context.Payments.AddAsync(payment);
		}
	}
}
