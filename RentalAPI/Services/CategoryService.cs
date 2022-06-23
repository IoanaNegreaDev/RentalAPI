using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
  
    public class CategoryService : BasicService<Category, ICategoryRepository>, ICategoryService
    {
        public CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork) 
            : base(repository, unitOfWork)
        {
        }

        public async Task<Category> FindByNameAsync(string categoryName)
              => await _repository.FindByNameAsync(categoryName); 
    }
}
