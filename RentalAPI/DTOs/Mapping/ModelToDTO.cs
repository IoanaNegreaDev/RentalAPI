using AutoMapper;
using RentalAPI.DTOs;
using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTO.Mapping
{
    public class RentalDamageCostResolver : IValueResolver<Rental, RentalDTO, float>
    {
        public float Resolve(Rental source, RentalDTO destination, float member, ResolutionContext context)
             => source.RentalDamages.Sum(item => item.Damage.DamageCost);
    }

    public class TotalBasePriceResolver : IValueResolver<Contract, ContractDTO, float>
    {
        public float Resolve(Contract source, ContractDTO destination, float member, ResolutionContext context)
            => source.Rentals.Sum(item => item.BasePrice);
    }

    public class TotalDamagePriceResolver : IValueResolver<Contract, ContractDTO, float>
    {
        public float Resolve(Contract source, ContractDTO destination, float member, ResolutionContext context)
            => source.Rentals.Sum(item => (float)(item.RentalDamages.Sum(item => item.Damage.DamageCost)));
    }

    public class TotalFullTankPriceResolver : IValueResolver<Contract, VehicleContractDTO, float>
    {
        public float Resolve(Contract source, VehicleContractDTO destination, float member, ResolutionContext context)
            => source.Rentals.Where(item => item.GetType().IsSubclassOf(typeof(Vehicle)) &&
                                         ((VehicleRental)item).FullTank == false)
                          .Sum(item => ((VehicleRental)item).FullTankPrice);
    }

    public class ModelToDTO : Profile
    {
        public ModelToDTO()
        {
            CreateMap<Rentable, RentableDTO>().IncludeAllDerived();
            CreateMap<RentableDTO, Rentable>().IncludeAllDerived();

            CreateMap<Vehicle, VehicleDTO>();
            CreateMap<VehicleDTO, Vehicle>();

            CreateMap<Truck, TruckDTO>();
            CreateMap<TruckDTO, Truck>();

            CreateMap<Minivan, MinivanDTO>();
            CreateMap<MinivanDTO, Minivan>();

            CreateMap<Sedan, SedanDTO>();
            CreateMap<SedanDTO, Sedan>();

            CreateMap<Client, ClientDTO>();
            CreateMap<ClientDTO, Client>();

            CreateMap<ClientCreationDTO, Client>();

            CreateMap<Payment, PaymentDTO>();
            CreateMap<PaymentDTO, Payment>();

            CreateMap<ContractCreationDTO, Contract>();
            CreateMap<Contract, ContractDTO>()
                .ForMember(dest => dest.TotalBasePriceInDefaultCurrency, 
                            opt => opt.MapFrom(new TotalBasePriceResolver())) 
                .ForMember(dest => dest.TotalDamagePriceInDefaultCurrency,
                            opt => opt.MapFrom(new TotalDamagePriceResolver()))
                .IncludeAllDerived();
            CreateMap<ContractDTO, Contract>().IncludeAllDerived();
            CreateMap<Contract, VehicleContractDTO>()
              .ForMember(dest => dest.TotalFullTankPriceInDefaultCurrency,
                          opt => opt.MapFrom(new TotalFullTankPriceResolver()));

            CreateMap<RentalCreationDTO, VehicleRental>();
            CreateMap<Rental, RentalDTO>()
                .ForMember(dest => dest.DamagePrice, opt => opt.MapFrom(new RentalDamageCostResolver()))
                .IncludeAllDerived();
            CreateMap<RentalDTO, Rental>().IncludeAllDerived();

            CreateMap<VehicleRental, VehicleRentalDTO>();
            CreateMap<VehicleRentalDTO, VehicleRental>();

            CreateMap<Damage, DamageDTO>();
            CreateMap<DamageDTO, Damage>();

            CreateMap<RentalDamage, RentalDamageDTO>();
            CreateMap<RentalDamageDTO, RentalDamage>();

            CreateMap<Domain, DomainDTO>();
            CreateMap<DomainDTO, Domain>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
        }
    }
}
