using RentalAPI.DTO;
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
    public class VehicleContractService: IVehicleContractService
    {
        private readonly IVehicleContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfWork;
        public VehicleContractService(IVehicleContractRepository contractRepository, IUnitOfWork unitOfWork)
        {
            this._contractRepository = contractRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VehicleContract>> ListAsync()
        {
            return await _contractRepository.ListAsync();
        }

        public async Task<VehicleContractOperationResponse> AddAsync(VehicleContract contract)
        {
            try
            {
                await _contractRepository.AddAsync(contract);
                await _unitOfWork.SaveChangesAsync();
                return new VehicleContractOperationResponse(contract);
            }
            catch (Exception ex)
            {
                return new VehicleContractOperationResponse("Failed to add contract to the database " + ex.Message);
            }
        }

        public async Task<VehicleContract> FindByIdAsync(int id)
        {
            return await _contractRepository.FindByIdAsync(id);
        }

        public async Task<VehicleContractOperationResponse> UpdateAsync(VehicleContract contract)
        {
            var existingContract = await _contractRepository.FindByIdAsync(contract.Id);

            if (existingContract == null)
                return new VehicleContractOperationResponse("Contract not found.");

            existingContract.TotalBasePriceInDefaultCurrency = contract.TotalBasePriceInDefaultCurrency;
            existingContract.TotalDamagePriceInDefaultCurrency = contract.TotalDamagePriceInDefaultCurrency;
            existingContract.TotalFullTankPriceInDefaultCurrency = contract.TotalFullTankPriceInDefaultCurrency;
            try
            {
                _contractRepository.Update(existingContract);
                await _unitOfWork.SaveChangesAsync();

                return new VehicleContractOperationResponse(existingContract);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new VehicleContractOperationResponse($"An error occurred when updating the contract: {ex.Message}");
            }
        }
    }
}
