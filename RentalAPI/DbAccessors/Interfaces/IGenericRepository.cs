using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> ListAsync();
        public Task<T> FindByIdAsync(int id);
        public Task AddAsync(T item);
        public void Update(T item);
        public void Remove(T item);
    }
}
