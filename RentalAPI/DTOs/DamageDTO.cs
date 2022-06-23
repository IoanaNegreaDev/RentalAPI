using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class DamageDTO
    {
        public int Id { get; set; }
        public int RentableItemId { get; set; }
        public int OccuredInRentalId { get; set; }
        public string DamageDescription { get; set; }
        public float DamageCost { get; set; }
    }
}
