using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class ContractCreationDTO
    {
        public int ClientId { get; set; }
        public float TotalBasePriceInDefaultCurrency { get; set; }
        public float TotalDamagePriceInDefaultCurrency { get; set; }
    }
}
