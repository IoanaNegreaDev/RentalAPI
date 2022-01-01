using RentalAPI.Models;
using RentalAPI.Services.DbOperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IVehicleRentalService
    {
        public Task<IEnumerable<VehicleRental>> ListAsync();
        public Task<VehicleRentalOperationResponse> AddAsync(VehicleRental rental);
        public Task<VehicleRental> FindByIdAsync(int id);
        public Task<VehicleRentalOperationResponse> UpdateAsync(VehicleRental rental);
    }
}
