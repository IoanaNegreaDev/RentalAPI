using AutoMapper;
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

            CreateMap<Fuel, FuelDTO>();
            CreateMap<FuelDTO, Fuel>();

            CreateMap<Currency, CurrencyDTO>();

            CreateMap<ContractCreationDTO, Contract>();
            CreateMap<Contract, ContractDTO>()
                .ForMember(dest => dest.TotalBasePriceInPaymentCurrency, 
                            opt => opt.MapFrom(new TotalBasePriceResolver())) 
                .ForMember(dest => dest.TotalDamagePriceInPaymentCurrency,
                            opt => opt.MapFrom(new TotalDamagePriceResolver()))
                .ForMember(dest => dest.TotalExtraChargesInPaymentCurrency,
                            opt => opt.MapFrom(new TotalExtraChargesResolver()))
                .ForMember(dest => dest.TotalPriceInPaymentCurrency,
                            opt => opt.MapFrom(new TotalPriceResolver()))
                .IncludeAllDerived();

            CreateMap<RentalCreationDTO, VehicleRental>();
            CreateMap<RentalCreationDTO, Rental>();

            CreateMap<Rental, RentalDTO>()
                .ForMember(dest => dest.DamagePrice, opt => opt.MapFrom(new RentalDamageCostResolver()))
                .IncludeAllDerived();

            CreateMap<RentalDTO, Rental>().IncludeAllDerived();
            CreateMap<Rental, RentalDTO>().IncludeAllDerived();

            CreateMap<VehicleRental, VehicleRentalDTO>();
            CreateMap<VehicleRentalDTO, VehicleRental>();

            CreateMap<VehicleRentalUpdateDTO, VehicleRental>();

            CreateMap<RentalDamage, RentalDamageDTO>();
            CreateMap<RentalDamageDTO, RentalDamage>();

            CreateMap<Damage, DamageDTO>();
            CreateMap<DamageDTO, Damage>();

            CreateMap<RentalDamageCreationDTO, RentalDamage>();
            CreateMap<DamageIndirectCreationDTO, Damage>();

            CreateMap<UserRegistrationDTO, UserCredentials>();
            CreateMap<UserLoginDTO, UserCredentials>();
            CreateMap<RentalUser, UserDTO>();
        }
    }
}
