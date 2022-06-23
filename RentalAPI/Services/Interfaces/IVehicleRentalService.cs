using RentalAPI.Models;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IVehicleRentalService:IBasicRentalService<VehicleRental>
    {
        Task<DbOperationResponse<IEnumerable<VehicleRental>>> ListAsync(string userId, int contractId);
        Task<DbOperationResponse<VehicleRental>> FindByIdAsync(string userId, int contractId, int id);
        public Task<DbOperationResponse<VehicleRental>> AddAsync(string userId, int contractId, VehicleRental rental);
        public Task<DbOperationResponse<VehicleRental>> UpdateAsync(string userId, int contractId, VehicleRental rental);
    }
}
