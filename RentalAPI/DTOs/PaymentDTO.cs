using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int PaymentCurrencyId { get; set; }
        public float TotalPriceInPaymentCurrency { get; set; }
        public float PaidAmountInPaymentCurrency { get; set; }
        virtual public ContractDTO Contract { get; set; }
        virtual public CurrencyDTO Currency { get; set; }
    }
}
