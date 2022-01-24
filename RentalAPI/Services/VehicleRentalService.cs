using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class VehicleRentalService : BaseRentalService<VehicleRental, IRentalRepository>, IVehicleRentalService
    {
        public VehicleRentalService(IVehicleRentalRepository vehicleRentalRepository, 
                                    IRentableRepository rentableRepository,
                                    IContractRepository contractRepository,
                                    IUnitOfWork unitOfWork) : 
            base (vehicleRentalRepository, rentableRepository, contractRepository, unitOfWork)
          
        {
        
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
