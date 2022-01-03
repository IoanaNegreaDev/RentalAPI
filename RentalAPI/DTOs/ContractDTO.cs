using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class ContractDTO
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public virtual ClientDTO Client { get; set; }
        virtual public CurrencyDTO Currency { get; set; }
        public float TotalBasePriceInPaymentCurrency { get; set; }
        public float TotalDamagePriceInPaymentCurrency { get; set; }
        public float TotalPriceInPaymentCurrency { get; set; }

        virtual public ICollection<RentalDTO> Rentals { get; set; }
    }
}
