using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class RentalService : BaseService<Rental, IRentalRepository>, IRentalService
    {
        private readonly IRentableRepository _rentableRepository;
        private readonly IContractRepository _contractRepository;

        public RentalService(IRentalRepository repository,
                                IRentableRepository rentableRepository,
                                IContractRepository contractRepository,
                                IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
            this._rentableRepository = rentableRepository;
            this._contractRepository = contractRepository;
        }

        override public async Task<DbOperationResponse<Rental>> AddAsync(Rental rental)
        {
            var rentedItem = await _rentableRepository.FindByIdAsync(rental.RentedItemId);
            if (rentedItem == null)
                return new DbOperationResponse<Rental>("Rented Item not found.");

            if (!await _rentableRepository.IsAvailable(rentedItem.Id, rental.StartDate, rental.EndDate))
                return new DbOperationResponse<Rental>("The item is already rented. Please change item and/or dates.");

            var contract = await _contractRepository.FindByIdAsync(rental.ContractId);
            if (contract == null)
                return new DbOperationResponse<Rental>("Contract not found.");

            rental.BasePrice = (float)(rentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);
      
            try
            {
                await _repository.AddAsync(rental);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<Rental>(rental);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<Rental>("Failed to add rental to the database " + ex.Message);
            }
        }

        override public async Task<DbOperationResponse<Rental>> UpdateAsync(Rental rental)
        {

            var existing = await _repository.FindByIdAsync(rental.Id);

            if (existing == null)
                return new DbOperationResponse<Rental>("Rental not found.");

            existing.StartDate = rental.StartDate;
            existing.EndDate = rental.EndDate;
            existing.BasePrice = (float)(existing.RentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);

            try
            {
                _repository.Update(existing);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<Rental>(existing);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<Rental>($"An error occurred when updating the rental: {ex.Message}");
            }
        }
    }
}
