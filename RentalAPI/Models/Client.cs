using System;
using System.Collections.Generic;

#nullable disable

namespace RentalAPI.Models
{
    public partial class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
    }
}
