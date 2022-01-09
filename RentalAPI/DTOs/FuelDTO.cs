using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class FuelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float PricePerUnit { get; set; }
    }
}
