using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class PaymentCreationDTO
    {
        public int ContractId { get; set; }
        public float PaidAmountInPaymentCurrency { get; set; }
    }
}
