using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Models
{
    [Table("VehicleContracts")]
    public class VehicleContract: Contract
    {
        public float TotalFullTankPriceInDefaultCurrency { get; set; }
    }
}
