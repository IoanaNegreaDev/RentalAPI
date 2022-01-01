using RentalAPI.Models;
using RentalAPI.Services.DbOperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IVehicleContractService
    {
        public Task<IEnumerable<VehicleContract>> ListAsync();
        public Task<VehicleContractOperationResponse> AddAsync(VehicleContract contract);
        public Task<VehicleContract> FindByIdAsync(int id);
        public Task<VehicleContractOperationResponse> UpdateAsync(VehicleContract contract);

    }
}
