using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Models
{
    [Table("RentalDamages")]
    public class RentalDamage
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int DamageId { get; set; }

        public virtual Damage Damage { get; set; }
        public virtual Rental Rental { get; set; }
    }
}
