using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class FuelType
    {
        public FuelType()
        {
            EngineTypes = new HashSet<EngineType>();
            PricesPerFuelUnits = new HashSet<PricesPerFuelUnit>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EngineType> EngineTypes { get; set; }
        public virtual ICollection<PricesPerFuelUnit> PricesPerFuelUnits { get; set; }
    }
}
