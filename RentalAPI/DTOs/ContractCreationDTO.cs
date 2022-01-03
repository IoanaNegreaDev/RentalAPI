using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class ContractCreationDTO
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string PaymentCurrency { get; set; }
    }
}
