using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Default { get; set; }

        virtual public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
