using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTO
{
    public class RentableDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public double PricePerDay { get; set; }

       // public virtual ICollection<RentalDTO> Rentals { get; set; }
    }

    public class VehicleDTO: RentableDTO
    {
        public string Producer { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int EngineTypeId { get; set; }
        public int? TankCapacity { get; set; }
    }
    public class TruckDTO: VehicleDTO
    {
        public int CargoCapacity { get; set; }
    }

    public class MinivanDTO : VehicleDTO
    {
        public int PassangersSeatsCount { get; set; }
    }

    public class SedanDTO: VehicleDTO
    {
        public string Color { get; set; }
    }
}
