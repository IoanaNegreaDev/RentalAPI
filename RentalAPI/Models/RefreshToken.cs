using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string RentalUserId { get; set; }
        public string Token { get; set; }

        public DateTime CreationDate { get; set; }  
        public DateTime ExpiryDate { get; set; }

        public DateTime RevokedOn { get; set; }

        public virtual RentalUser User { get; set; }
    }
}
