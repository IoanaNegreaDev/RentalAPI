using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class VehicleContractDTO:ContractDTO
    {
        public float TotalFullTankPriceInDefaultCurrency { get; set; }
    }
}
