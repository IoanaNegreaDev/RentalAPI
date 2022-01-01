using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IVehicleRentalRepository
    {
        public Task<IEnumerable<VehicleRental>> ListAsync();
        public Task<VehicleRental> FindByIdAsync(int id);
        public Task AddAsync(VehicleRental rental);
        public void Update(VehicleRental rental);
    }
}
