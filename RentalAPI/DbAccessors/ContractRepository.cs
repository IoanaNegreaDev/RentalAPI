﻿using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class ContractRepository: GenericRepository<Contract>, IContractRepository
    {
		public ContractRepository(RentalDbContext context) : base(context)
		{
		}

		public async Task<Contract> FindByIdAsync(string userId, int id)
			=> await _table
			.Where(item => item.Id == id && item.User.Id == userId)
			.Include(item => item.Rentals)
				.ThenInclude(item => item.RentalDamages)
			.Include(item => item.Rentals)
				.ThenInclude(item => item.RentedItem)
			.Include(item => item.Currency)
			.FirstOrDefaultAsync();

		override public async Task<Contract> FindByIdAsync(int id)
			=> await _table
					.Where(item => item.Id == id)
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentalDamages)
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentedItem)
					.Include(item => item.Currency)
					.FirstOrDefaultAsync();
		override public async Task<IEnumerable<Contract>> ListAsync()
			=> await _table
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentalDamages)
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentedItem)
					.Include(item => item.User)
					.Include(item=>item.Currency)
					.ToListAsync();

		public async Task<IEnumerable<Contract>> ListAsync(string userId)
			=> await _table
					.Where(contract => contract.User.Id == userId)
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentalDamages)
					.Include(item => item.Rentals)
						.ThenInclude(item => item.RentedItem)
					.Include(item => item.User)
					.Include(item => item.Currency)
					.ToListAsync();



		public async Task RemoveAsync(Contract item)
		{
			var itemWithRentals = await _table
				.Where(c =>c.Id == item.Id)
				.Include(item => item.Rentals)
				.FirstOrDefaultAsync();
			 _table.Remove(itemWithRentals);
		}
	}
}