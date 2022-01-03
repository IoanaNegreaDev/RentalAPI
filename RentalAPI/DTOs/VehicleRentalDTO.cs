using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class VehicleRentalDTO: RentalDTO
    {
        public bool FullTank { get; set; }
        public float FullTankPrice { get; set; } // calculated
    }
}
