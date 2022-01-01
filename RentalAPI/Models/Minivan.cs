using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RentalAPI.Models
{
    [Table("Minivans")]
    public class Minivan: Vehicle
    {
        public int PassangersSeatsCount { get; set; }
    }
}
