using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class RentalDTO
    {
        public int Id { get; set; }
        public int RentedItemId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ContractId { get; set; }
        public float BasePrice { get; set; } // calculated
        public float DamagePrice { get; set; }// calculated
    }
}
