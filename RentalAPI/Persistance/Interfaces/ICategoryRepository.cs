using RentalAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> ListAsync();

        Task<Category> FindAsync(int id);

        Task<Category> FindAsync(string categoryName);
    }
}
