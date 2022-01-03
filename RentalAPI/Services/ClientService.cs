using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class ClientService:BaseService<Client, IClientRepository>, IClientService
    {    
        public ClientService(IClientRepository repository, IUnitOfWork unitOfWork) 
            : base(repository, unitOfWork)
        {
        }

        override public async Task<DbOperationResponse<Client>> UpdateAsync(Client client)
        {
            var existingClient = await _repository.FindByIdAsync(client.Id);

            if (existingClient == null)
                return new DbOperationResponse<Client>("Client not found.");

            existingClient.Name = client.Name;
            existingClient.Mobile = client.Mobile;

            try
            {
                _repository.Update(existingClient);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<Client>(existingClient);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new DbOperationResponse<Client>($"An error occurred when updating the client: {ex.Message}");
            }
        }

        public async Task<Client> FindByNameAsync(string name)
           => await _repository.FindByNameAsync(name);
    }
}
