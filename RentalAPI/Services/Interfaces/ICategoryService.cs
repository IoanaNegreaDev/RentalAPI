using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> ListAsync();
        Task<Category> FindAsync(int id);
        Task<Category> FindAsync(string categoryName);
    }
}
