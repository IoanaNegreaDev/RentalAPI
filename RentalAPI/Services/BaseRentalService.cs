using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services
{

    public class BaseRentalService<T, TRepository>: BasicService<Rental, IRentalRepository>, IBaseRentalService<T> 
        where T : Rental
        where TRepository : IRentalRepository
    {
        protected readonly IRentableRepository _rentableRepository;
        protected readonly IContractRepository _contractRepository;

        public BaseRentalService(TRepository rentalRepository,
                        IRentableRepository rentableRepository,
                        IContractRepository contractRepository,
                        IUnitOfWork unitOfWork) : base(rentalRepository, unitOfWork)
        {
            _rentableRepository = rentableRepository;
            _contractRepository = contractRepository;
        }

       // public event EventHandler<EventArgs> RentalsChanged;
       /* protected virtual void OnRentalsChanged()
        {
            RentalsChanged?.Invoke(this, null);
        }*/
        virtual public async Task<DbOperationResponse<IEnumerable<T>>> ListAsync(int contractId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<IEnumerable<T>>("Contract not found.");

            var rentals = await _repository.ListAsync(contractId);
            if (rentals == null)
                return new DbOperationResponse<IEnumerable<T>>("No rentals.");

            return new DbOperationResponse<IEnumerable<T>>(rentals as IEnumerable<T>);
        }

        virtual public async Task<DbOperationResponse<T>> FindByIdAsync(int contractId, int rentalId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<T>("Contract not found.");

            var rental = await _repository.FindByIdAsync(contractId, rentalId);
            if (rental == null)
                return new DbOperationResponse<T>("Rental not found.");

            return new DbOperationResponse<T>(rental as T);
        }

        virtual public async Task<DbOperationResponse<T>> AddAsync(int contractId, T rental)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<T>("Contract not found.");

            var rentedItem = await _rentableRepository.FindByIdAsync(rental.RentedItemId);
            if (rentedItem == null)
                return new DbOperationResponse<T>("Rented Item not found.");

            if (!await _rentableRepository.IsAvailable(rentedItem.Id, rental.StartDate, rental.EndDate))
                return new DbOperationResponse<T>("The item is already rented. Please change item and/or dates.");

            rental.ContractId = contractId;
            rental.BasePrice = (float)(rentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);

            try
            {
                await _repository.AddAsync(rental);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<T>(rental);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<T>("Failed to add rental to the database " + ex.Message);
            }
        }

        virtual public async Task<DbOperationResponse<T>> UpdateAsync(int contractId, T rental)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<T>("Contract not found.");

            var rentedItem = await _rentableRepository.FindByIdAsync(rental.RentedItemId);
            if (rentedItem == null)
                return new DbOperationResponse<T>("Rented Item not found.");

            var existing = await _repository.FindByIdAsync(contractId, rental.Id);
            if (existing == null)
                return new DbOperationResponse<T>("Rental not found.");

            existing.StartDate = rental.StartDate;
            existing.EndDate = rental.EndDate;
            existing.BasePrice = (float)(existing.RentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);

            try
            {
                _repository.Update(existing);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<T>(existing as T);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<T>($"An error occurred when updating the rental: {ex.Message}");
            }
        }

        virtual public async Task<DbOperationResponse<T>> DeleteAsync(int contractId, int rentalId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<T>("Contract not found.");

            var item = await _repository.FindByIdAsync(contractId, rentalId);

            if (item == null)
                return new DbOperationResponse<T>("Item not found.");

            try
            {
                _repository.Remove(item);
                await _unitOfWork.SaveChangesAsync();

                var updatedContract = await _contractRepository.FindByIdAsync(contractId);

                return new DbOperationResponse<T>("Delete succeeded.");
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<T>($"An error occurred when deleting the item: {ex.Message}");
            }
        }

    }
}
