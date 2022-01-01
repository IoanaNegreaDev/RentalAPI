using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.DbOperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class VehicleRentalService: IVehicleRentalService
    {
        private readonly IVehicleRentalRepository _rentalRepository;
        private readonly IRentableRepository _rentableRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VehicleRentalService(IVehicleRentalRepository rentalRepository, 
                                    IRentableRepository rentableReposotory,
                                    IUnitOfWork unitOfWork)
        {
            this._rentalRepository = rentalRepository;
            this._rentableRepository = rentableReposotory;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VehicleRental>> ListAsync()
        {
            return await _rentalRepository.ListAsync();
        }

        public async Task<VehicleRentalOperationResponse> AddAsync(VehicleRental rental)
        {
            try
            {
                var rentedItem = await _rentableRepository.FindByIdAsync(rental.RentedItemId);
                if (rentedItem == null)
                    return new VehicleRentalOperationResponse("Failed to add rental to the database. RentedItemId not found.");

                rental.BasePrice = (float)(rentedItem.PricePerDay * (rental.EndDate - rental.StartDate).TotalDays);
                rental.DamagePrice = (float)0.0;
                rental.StatusId = 1;

                await _rentalRepository.AddAsync(rental);
                await _unitOfWork.SaveChangesAsync();

                return new VehicleRentalOperationResponse(rental);
            }
            catch (Exception ex)
            {
                return new VehicleRentalOperationResponse("Failed to add rental to the database " + ex.Message);
            }
        }

        public async Task<VehicleRental> FindByIdAsync(int id)
        {
            return await _rentalRepository.FindByIdAsync(id);
        }

        public async Task<VehicleRentalOperationResponse> UpdateAsync(VehicleRental rental)
        {
          
            var existing = await _rentalRepository.FindByIdAsync(rental.Id);

            if (existing == null)
                return new VehicleRentalOperationResponse("Rental not found.");

            existing.DamagePrice = rental.DamagePrice;
            existing.BasePrice = rental.BasePrice;

            try
            {
                _rentalRepository.Update(existing);
                await _unitOfWork.SaveChangesAsync();

                return new VehicleRentalOperationResponse(existing);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new VehicleRentalOperationResponse($"An error occurred when updating the rental: {ex.Message}");
            }
        }
    }
}
