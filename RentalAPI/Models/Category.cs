using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Rentable> Rentables { get; set; } = new HashSet<Rentable>();
        public virtual Domain Domain { get; set; }
    }
}
