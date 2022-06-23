using AutoMapper;
using RentalAPI.DTOs;
using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Mapping
{
    public class TotalDamagePriceResolver : IValueResolver<Contract, ContractDTO, float>
    {
        public float Resolve(Contract source, ContractDTO destination, float member, ResolutionContext context)
            => source.Rentals
                    .Where(item => item != null)
                    .Sum(item => (float)(item.RentalDamages.Sum(item => item.DamageCost))) * source.ExchangeRate;
    }
}
