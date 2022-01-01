using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class Damage
    {
        public Damage()
        {
            RentalDamages = new HashSet<RentalDamage>();
        }

        public int Id { get; set; }
        public int RentableItemId { get; set; }
        public string DamageDescription { get; set; }
        public float DamageCost { get; set; }

        public virtual Rentable Rentable { get; set; }
        public virtual ICollection<RentalDamage> RentalDamages { get; set; }
    }
}
