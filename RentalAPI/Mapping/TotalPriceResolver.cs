using AutoMapper;
using RentalAPI.DTOs;
using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Mapping
{
    public class TotalPriceResolver : IValueResolver<Contract, ContractDTO, float>
    {
        public float Resolve(Contract source, ContractDTO destination, float member, ResolutionContext context)
        {
            var basePrice = source.Rentals.Sum(item => item.BasePrice) * source.ExchangeRate;

            var damage = source.Rentals.Sum(item => (float)(item.RentalDamages
                            .Sum(item => item.DamageCost))) * source.ExchangeRate;

            var fullTank = source.Rentals.Where(item => item.GetType() == typeof(VehicleRental) &&
                          ((VehicleRental)item).FullTank == false)
                          .Sum(item => ((VehicleRental)item).FullTankPrice) * source.ExchangeRate;

            return basePrice + damage + fullTank;
        }
    }
}
