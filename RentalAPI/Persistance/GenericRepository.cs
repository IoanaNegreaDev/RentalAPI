using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class GenericRepository<T>: IGenericRepository<T> where T: class
    {
		protected DbSet<T> _table = null;
		protected readonly RentalDbContext _context;
		public GenericRepository(RentalDbContext context)
		{
			_context = context;
			_table = _context.Set<T>();			
		}

		virtual public async Task<IEnumerable<T>> ListAsync()
			=> await _table.ToListAsync();
		virtual public async Task<T> FindByIdAsync(int id)
			=> await _table.FindAsync(id);
		virtual public async Task AddAsync(T item)
			=> await _table.AddAsync(item);
		virtual public void Update(T item)
			=> _table.Update(item);
    }
}
