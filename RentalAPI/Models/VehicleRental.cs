using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("VehicleRental")]
    public class VehicleRental: Rental
    {
        public bool? FullTank { get; set; }
        public double? FullTankPrice { get; set; } // calculated
    }
}
