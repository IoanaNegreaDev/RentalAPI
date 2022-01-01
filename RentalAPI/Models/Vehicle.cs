using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("Vehicles")]
    public class Vehicle: Rentable
    {
        public string Producer { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int EngineTypeId { get; set; }
        public int? TankCapacity { get; set; }

        public virtual EngineType EngineType { get; set; }
      }
}
