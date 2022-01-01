using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
  
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _categoryRepository.ListAsync();
        }

        public async Task<Category> FindAsync(int id)
        {
            return await _categoryRepository.FindAsync(id);
        }

        public async Task<Category> FindAsync(string categoryName)
        {
            return await _categoryRepository.FindAsync(categoryName);
        }
    }
}
