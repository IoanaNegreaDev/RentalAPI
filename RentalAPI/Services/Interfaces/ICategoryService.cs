using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface ICategoryService: IBaseService<Category>
    {
        public Task<Category> FindByNameAsync(string categoryName);
    }
}
