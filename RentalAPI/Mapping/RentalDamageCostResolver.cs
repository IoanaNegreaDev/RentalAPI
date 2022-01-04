using AutoMapper;
using RentalAPI.DTOs;
using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Mapping
{
    public class RentalDamageCostResolver : IValueResolver<Rental, RentalDTO, float>
    {
        public float Resolve(Rental source, RentalDTO destination, float member, ResolutionContext context)
             => source.RentalDamages
                    .Where(item => item != null && item.Damage != null)
                    .Sum(item => item.Damage.DamageCost);
    }
}
