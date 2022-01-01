using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class PricesPerFuelUnit
    {
        public int Id { get; set; }
        public int FuelId { get; set; }
        public float PricePerUnit { get; set; }

        public virtual FuelType Fuel { get; set; }
    }
}
