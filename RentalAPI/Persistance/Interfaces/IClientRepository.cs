using RentalAPI.Models;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IClientRepository:IGenericRepository<Client>
    {
        public Task<Client> FindByNameAsync(string name);
    }
}
