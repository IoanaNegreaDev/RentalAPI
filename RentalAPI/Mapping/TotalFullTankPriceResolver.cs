using AutoMapper;
using RentalAPI.DTOs;
using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Mapping
{
    public class TotalFullTankPriceResolver : IValueResolver<Contract, VehicleContractDTO, float>
    {
        public float Resolve(Contract source, VehicleContractDTO destination, float member, ResolutionContext context)
            => source.Rentals
                       .Where(item => item != null &&
                                      item.GetType() == typeof(VehicleRental) &&
                                     ((VehicleRental)item).FullTank == false)
                       .Sum(item => ((VehicleRental)item).FullTankPrice)
                       * source.ExchangeRate;
    }
}
