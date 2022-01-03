using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class VehicleRentalService: BaseService<VehicleRental, IVehicleRentalRepository>, IVehicleRentalService
    {
        private readonly IRentableRepository _rentableRepository;
        private readonly IContractRepository _contractRepository;

        public VehicleRentalService(IVehicleRentalRepository repository, 
                                    IRentableRepository rentableRepository,
                                    IContractRepository contractRepository,
                                    IUnitOfWork unitOfWork)
            :base (repository, unitOfWork)
        {
            this._rentableRepository = rentableRepository;
            this._contractRepository = contractRepository;
        }
 
        override public async Task<DbOperationResponse<VehicleRental>> AddAsync(VehicleRental rental)
        {
            if (rental.StartDate > rental.EndDate)
                return new DbOperationResponse<VehicleRental>("Start date should be smaller than end date of the rental.");
            if (rental.StartDate < DateTime.Today)
                return new DbOperationResponse<VehicleRental>("Start date must be egual or bigger with tomorow's date.");

            var rentedItem = await _rentableRepository.FindByIdAsync(rental.RentedItemId);
            if (rentedItem == null)
                return new DbOperationResponse<VehicleRental>("Rented Item not found.");

            if (!await _rentableRepository.IsAvailable(rentedItem.Id, rental.StartDate, rental.EndDate))
                return new DbOperationResponse<VehicleRental>("The item is already rented.");
           
            var contract = _contractRepository.FindByIdAsync(rental.ContractId);
            if (contract == null)
                return new DbOperationResponse<VehicleRental>("Contract not found.");

            try
            {               
                rental.BasePrice = (float)(rentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);
                rental.FullTank = false;
                rental.FullTankPrice = (float)(rentedItem.TankCapacity * rentedItem.Fuel.PricePerUnit);

                await _repository.AddAsync(rental);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<VehicleRental>(rental);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<VehicleRental>("Failed to add rental to the database " + ex.Message);
            }
        }

        override public async Task<DbOperationResponse<VehicleRental>> UpdateAsync(VehicleRental rental)
        {
          
            var existing = await _repository.FindByIdAsync(rental.Id);

            if (existing == null)
                return new DbOperationResponse<VehicleRental>("Rental not found.");

            try
            {
                _repository.Update(existing);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<VehicleRental>(existing);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new DbOperationResponse<VehicleRental>($"An error occurred when updating the rental: {ex.Message}");
            }
        }
    }
}
