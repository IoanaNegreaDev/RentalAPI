using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class ClientService:IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            this._clientRepository = clientRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Client>> ListAsync()
        {
            return await _clientRepository.ListAsync();
        }

        public async Task<ClientOperationResponse> AddAsync(Client client)
        {
            try
            {
                await _clientRepository.AddAsync(client);
                await _unitOfWork.SaveChangesAsync();
                return new ClientOperationResponse(client);
            }
            catch (Exception ex)
            {
                return new ClientOperationResponse("Failed to add client to the database " + ex.Message);
            }
        }
        public async Task<ClientOperationResponse> UpdateAsync(Client client)
        {
            var existingClient = await _clientRepository.FindByIdAsync(client.Id);

            if (existingClient == null)
                return new ClientOperationResponse("Client not found.");

            existingClient.Name = client.Name;
            existingClient.Mobile = client.Mobile;

            try
            {
                _clientRepository.Update(existingClient);
                await _unitOfWork.SaveChangesAsync();

                return new ClientOperationResponse(existingClient);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ClientOperationResponse($"An error occurred when updating the client: {ex.Message}");
            }
        }

        public async Task<Client> FindByIdAsync(int id)
        {
            return await _clientRepository.FindByIdAsync(id);
        }
        public async Task<Client> FindByNameAsync(string name)
        {
            return await _clientRepository.FindByNameAsync(name);
        }

    }
}
