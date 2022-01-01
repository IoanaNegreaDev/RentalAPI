using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("Trucks")]
    public class Truck: Vehicle
    {
        public int CargoCapacity { get; set; }
    }
}
