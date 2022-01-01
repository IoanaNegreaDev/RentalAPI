using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("Sedans")]
    public class Sedan:Vehicle
    {
        public string Color { get; set; }
    }
}
