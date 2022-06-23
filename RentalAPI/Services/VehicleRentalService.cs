using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RentalAPI.Services
{
    public class VehicleRentalService : BasicRentalService<VehicleRental, IRentalRepository>, IVehicleRentalService
    {
        public VehicleRentalService(IVehicleRentalRepository vehicleRentalRepository, 
                                    IRentableRepository rentableRepository,
                                    IContractRepository contractRepository,
                                    IUnitOfWork unitOfWork) : 
            base (vehicleRentalRepository, rentableRepository, contractRepository, unitOfWork)
          
        {
        
        }

        private async Task<BasicOperationResponse<bool>> CheckConsistencyAsync(string userId, int contractId, int? rentalId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new BasicOperationResponse<bool>("Contract not found.");

            if (userId != contract.User.Id)
                return new BasicOperationResponse<bool>("Invalid contractId for this user.");

            if (rentalId != null)
            {
                var contractContainsRental = contract.Rentals.Any(rental => rental.Id == rentalId);
                if (!contractContainsRental)
                    return new BasicOperationResponse<bool>("Invalid rentalId for this contract.");
            }

            return new BasicOperationResponse<bool>(true);
        }

        public async Task<DbOperationResponse<IEnumerable<VehicleRental>>> ListAsync(string userId, int contractId)
        {
            var checkConsistencyResult = await CheckConsistencyAsync(userId, contractId, null);
            if (!checkConsistencyResult.Success)
                return new DbOperationResponse<IEnumerable<VehicleRental>>(checkConsistencyResult.Message);
          
            var rentals = await _repository.ListAsync(contractId);
            if (rentals == null)
                return new DbOperationResponse<IEnumerable<VehicleRental>>("No rentals.");

            return new DbOperationResponse<IEnumerable<VehicleRental>>(rentals as IEnumerable<VehicleRental>);
        }
        public async Task<DbOperationResponse<VehicleRental>> FindByIdAsync(string userId, int contractId, int rentalId)
        {
            var checkConsistencyResult = await CheckConsistencyAsync(userId, contractId, rentalId);
            if (!checkConsistencyResult.Success)
                return new DbOperationResponse<VehicleRental>(checkConsistencyResult.Message);

            var rental = await _repository.FindByIdAsync(rentalId);
            if (rental == null)
                return new DbOperationResponse<VehicleRental>("Rental not found.");

            return new DbOperationResponse<VehicleRental>(rental as VehicleRental);
        }
        public async Task<DbOperationResponse<VehicleRental>> AddAsync(string userId, int contractId, VehicleRental rental)
        {
            var checkConsistencyResult = await CheckConsistencyAsync(userId, contractId, null);
            if (!checkConsistencyResult.Success)
                return new DbOperationResponse<VehicleRental>(checkConsistencyResult.Message);

            var rentedItem = await _rentableRepository.FindByIdAsync(rental.RentedItemId);
            if (rentedItem == null)
                return new DbOperationResponse<VehicleRental>("Rented Item not found.");

            if (!await _rentableRepository.IsAvailable(rentedItem.Id, rental.StartDate, rental.EndDate))
                return new DbOperationResponse<VehicleRental>("The item is already rented. Please change item and/or dates.");

            rental.ContractId = contractId;
            rental.BasePrice = (float)(rentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);

            try
            {
                await _repository.AddAsync(rental);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<VehicleRental>(rental);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<VehicleRental>("Failed to add rental to the database " + ex.Message);
            }
        }
        public async Task<DbOperationResponse<VehicleRental>> UpdateAsync(string userId, int contractId, VehicleRental rental)
        {
            var checkConsistencyResult = await CheckConsistencyAsync(userId, contractId, null);
            if (!checkConsistencyResult.Success)
                return new DbOperationResponse<VehicleRental>(checkConsistencyResult.Message);


            var existing = await _repository.FindByIdAsync(contractId, rental.Id);
            if (existing == null)
                return new DbOperationResponse<VehicleRental>("Rental not found.");

            existing.StartDate = rental.StartDate;
            existing.EndDate = rental.EndDate;
            existing.BasePrice = (float)(existing.RentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);

            try
            {
                _repository.Update(existing);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<VehicleRental>(existing as VehicleRental);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<VehicleRental>($"An error occurred when updating the rental: {ex.Message}");
            }
        }

        override public async Task<DbOperationResponse<VehicleRental>> AddAsync(int contractId, VehicleRental rental)
        {
            var rentedItem = await _rentableRepository.FindByIdAsync(rental.RentedItemId);
            if (rentedItem == null)
                return new DbOperationResponse<VehicleRental>("Rented Item not found.");

            if (!await _rentableRepository.IsAvailable(rentedItem.Id, rental.StartDate, rental.EndDate))
                return new DbOperationResponse<VehicleRental>("The item is already rented. Please change item and/or dates.");
           
            var contract = await _contractRepository.FindByIdAsync(rental.ContractId);
            if (contract == null)
                return new DbOperationResponse<VehicleRental>("Contract not found.");

            rental.BasePrice = (float)(rentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);
            rental.FullTank = false;
            rental.FullTankPrice = (float)(((Vehicle)rentedItem).TankCapacity * ((Vehicle)rentedItem).Fuel.PricePerUnit);

            try
            {              
                await _repository.AddAsync(rental);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<VehicleRental>(rental);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<VehicleRental>("Failed to add vehicle rental to the database " + ex.Message);
            }
        }
    }
}
