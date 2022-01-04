using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("Rentables")]
    public class Rentable
    {
        public int Id { get; init; }
        public int CategoryId { get; set; }
        public float PricePerDay { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; } = new HashSet<Rental>();
        public virtual ICollection<Damage> Damages { get; set; } = new HashSet<Damage>();
    }
}
