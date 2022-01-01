using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class EngineType
    {
        public EngineType()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int FuelId { get; set; }

        public virtual FuelType Fuel { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
