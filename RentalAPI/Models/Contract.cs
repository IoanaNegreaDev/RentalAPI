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

        virtual public Currency Currency { get; set; }
        virtual public ICollection<Rental> Rentals { get; set; } = new HashSet<Rental>();
    }
}
