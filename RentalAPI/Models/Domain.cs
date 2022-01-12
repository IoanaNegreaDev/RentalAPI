using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class Domain
    {
        public int Id { get; set; }
        public string Name { get; set; }

    //    public virtual ICollection<Rentable> Rentables { get; set; } = new HashSet<Rentable>();
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    }
}
