using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class VehicleRentalUpdateDTO:RentalUpdateDTO
    {
        public bool FullTank { get; set; }
    }
}
