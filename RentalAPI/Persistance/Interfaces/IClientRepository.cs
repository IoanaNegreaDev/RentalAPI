using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IClientRepository
    {
        public Task<IEnumerable<Client>> ListAsync();
        public Task<Client> FindByIdAsync(int id);
        public Task<Client> FindByNameAsync(string name);
        public Task AddAsync(Client client);
        public void Update(Client client);
    }
}
