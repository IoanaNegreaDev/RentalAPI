using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(RentalDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _context.Categories.Include(item => item.Domain).ToListAsync();
        }

        public async Task<Category> FindAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> FindAsync(string categoryName)
        {
            return await _context.Categories
                        .Where(item => item.Name.ToLower().Contains(categoryName.ToLower()))
                        .FirstOrDefaultAsync();
        }


    }
}
