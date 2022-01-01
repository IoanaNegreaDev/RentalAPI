using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(RentalDbContext context) : base(context)
        {
        }

        override public async Task<IEnumerable<Category>> ListAsync()
            => await _table.Include(item => item.Domain).ToListAsync();
   
        virtual public async Task<Category> FindByNameAsync(string categoryName)
            => await _table.Where(item => item.Name.ToLower().Contains(categoryName.ToLower()))
                        .FirstOrDefaultAsync();
    }
}
