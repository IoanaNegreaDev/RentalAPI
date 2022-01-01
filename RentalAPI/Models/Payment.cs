using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("Payments")]
    public class Payment
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int PaymentCurrencyId { get; set; }
        public float TotalPriceInPaymentCurrency { get; set; }
        public float PaidAmountInPaymentCurrency { get; set; }

        virtual public Contract Contract { get; set; }
        virtual public Currency Currency {get; set;}
    }
}
