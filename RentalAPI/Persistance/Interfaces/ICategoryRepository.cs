using RentalAPI.Models;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        Task<Category> FindByNameAsync(string categoryName);
    }
}
