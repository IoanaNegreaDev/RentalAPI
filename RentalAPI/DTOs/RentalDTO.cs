using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class RentalDTO
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime EndDate { get; set; }
           
        public int ContractId { get; set; }
        public float BasePrice { get; set; }
        public float DamagePrice { get; set; }

        public virtual RentableDTO RentedItem { get; set; }
        public virtual ICollection<RentalDamageDTO> RentalDamages { get; set; }
    }
}
