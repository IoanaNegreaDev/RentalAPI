using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class Fuel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float PricePerUnit { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
    }
}
