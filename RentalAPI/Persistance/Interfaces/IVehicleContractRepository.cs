using RentalAPI.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IVehicleContractRepository
    {
        public Task<IEnumerable<VehicleContract>> ListAsync();
        public Task<VehicleContract> FindByIdAsync(int id);
        public Task AddAsync(VehicleContract contract);
        public void Update(VehicleContract contract);
    }
}
