using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class ContractCreationDTO
    {
        public virtual ClientCreationDTO Client { get; set; }
        public int PaymentCurrencyId { get; set; }
    }
}
