using RentalAPI.Models;
using RentalAPI.Services.OperationStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IClientService
    {
        public Task<IEnumerable<Client>> ListAsync();
        public Task<ClientOperationResponse> AddAsync(Client client);
        public Task<ClientOperationResponse> UpdateAsync(Client client);
        public Task<Client> FindByIdAsync(int id);
        public Task<Client> FindByNameAsync(string name);
    }
}
