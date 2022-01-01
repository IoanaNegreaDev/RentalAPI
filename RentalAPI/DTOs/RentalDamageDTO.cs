using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class RentalDamageDTO
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public virtual DamageDTO Damage { get; set; }
    }
}
