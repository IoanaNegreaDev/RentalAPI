using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Models
{
    public class RentalStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        virtual public ICollection<Rental> Rentals { get; set; } = new HashSet<Rental>();
    }
}
