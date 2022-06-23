using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class Damage
    {
        public int Id { get; set; }
        public int RentableItemId { get; set; }
        public int OccuredInRentalId { get; set; }
        public string DamageDescription { get; set; }
        public float DamageCost { get; set; }

        public virtual Rentable Rentable { get; set; }
        public virtual Rental Rental { get; set; }
    }
}
