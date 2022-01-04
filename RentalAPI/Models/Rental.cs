using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("Rentals")]
    public partial class Rental
    {
        public int Id { get; set; }
        public int RentedItemId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ContractId { get; set; }
        public float BasePrice { get; set; }

        public virtual Rentable RentedItem { get; set; } 
        public virtual Contract Contract { get; set; }
        public virtual ICollection<RentalDamage> RentalDamages { get; set; } = new HashSet<RentalDamage>();
    }
}
