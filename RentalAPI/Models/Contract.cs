﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public float TotalBasePriceInDefaultCurrency { get; set; }
        public float TotalDamagePriceInDefaultCurrency { get; set; }

        public virtual Client Client { get; set; }
        virtual public ICollection<Rental> Rentals { get; set; } = new HashSet<Rental>();
        virtual public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}