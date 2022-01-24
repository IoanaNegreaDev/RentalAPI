using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Models
{
    public class Contract
    {
        public int Id { get; set; }    
        public DateTime CreationDate { get; set; }
        public int PaymentCurrencyId { get; set; }
        public float ExchangeRate { get; set; }
        public RentalUser User { get; set; }

   /*     #region CalculatedFields
        public float TotalBasePriceInPaymentCurrency { get; set; }
        public float TotalDamagePriceInPaymentCurrency { get; set; }
        public float TotalExtraChargesInPaymentCurrency { get; set; }
        public float TotalPriceInPaymentCurrency { get; set; }
        #endregion*/

        virtual public Currency Currency { get; set; }
        virtual public ICollection<Rental> Rentals { get; set; } = new HashSet<Rental>();
    }
}
