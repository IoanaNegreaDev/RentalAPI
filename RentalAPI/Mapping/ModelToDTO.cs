using AutoMapper;
using RentalAPI.DTO;
using RentalAPI.DTOs;
using RentalAPI.Models;


namespace RentalAPI.Mapping
{
    public class ModelToDTO : Profile
    {
        public ModelToDTO()
        {
            CreateMap<Domain, DomainDTO>();
            CreateMap<DomainDTO, Domain>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

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
            CreateMap<ClientUpdateDTO, Client>();

            CreateMap<Currency, CurrencyDTO>();

            CreateMap<ContractCreationDTO, Contract>();
            CreateMap<ContractCreationDTO, Client>();
            CreateMap<Contract, ContractDTO>()
                .ForMember(dest => dest.TotalBasePriceInPaymentCurrency, 
                            opt => opt.MapFrom(new TotalBasePriceResolver())) 
                .ForMember(dest => dest.TotalDamagePriceInPaymentCurrency,
                            opt => opt.MapFrom(new TotalDamagePriceResolver()))
                .ForMember(dest => dest.TotalPriceInPaymentCurrency,
                            opt => opt.MapFrom(new TotalPriceResolver()))
                .IncludeAllDerived();
            CreateMap<ContractDTO, Contract>().IncludeAllDerived();
            CreateMap<Contract, VehicleContractDTO>()
              .ForMember(dest => dest.TotalFullTankPriceInPaymentCurrency,
                          opt => opt.MapFrom(new TotalFullTankPriceResolver()));

            CreateMap<RentalCreationDTO, VehicleRental>();
            
            CreateMap<Rental, RentalDTO>()
                .ForMember(dest => dest.DamagePrice, opt => opt.MapFrom(new RentalDamageCostResolver()))
                .IncludeAllDerived();

            CreateMap<RentalDTO, Rental>().IncludeAllDerived();

            CreateMap<VehicleRental, VehicleRentalDTO>();
            CreateMap<VehicleRentalDTO, VehicleRental>();
            
            CreateMap<RentalDamage, RentalDamageDTO>();
            CreateMap<RentalDamageDTO, RentalDamage>();

            CreateMap<Damage, DamageDTO>();
            CreateMap<DamageDTO, Damage>();

            CreateMap<RentalDamageCreationDTO, RentalDamage>();
            CreateMap<DamageIndirectCreationDTO, Damage>();
        }
    }
}
