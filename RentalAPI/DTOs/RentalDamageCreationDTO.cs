﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTOs
{
    public class RentalDamageCreationDTO
    {
        public int RentalId { get; set; }
        public string DamageDescription { get; set; }
        public float DamageCost { get; set; }
    }
}