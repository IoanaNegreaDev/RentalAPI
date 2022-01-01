using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("Rentals")]
    public partial class Rental
    {
        public Rental()
        {
            RentalDamages = new HashSet<RentalDamage>();
        }

        public int Id { get; set; }
        public int RentedItemId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ContractId { get; set; }
        public int StatusId { get; set; } // internal

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public float BasePrice { get; set; }// calculated
    
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public float DamagePrice { get; set;  }// calculated

        public virtual Rentable RentedItem { get; set; } 
        public virtual Contract Contract { get; set; }
        public virtual RentalStatus Status { get; set; }
        public virtual ICollection<RentalDamage> RentalDamages { get; set; }
    }
}
