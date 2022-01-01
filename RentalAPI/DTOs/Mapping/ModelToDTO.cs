using AutoMapper;
using RentalAPI.DTOs;
using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.DTO.Mapping
{
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

            CreateMap<Contract, ContractDTO>().IncludeAllDerived();
            CreateMap<ContractDTO, Contract>().IncludeAllDerived();

            CreateMap<ContractCreationDTO, Contract>().IncludeAllDerived();
            CreateMap<VehicleContractCreationDTO, Contract>();

            CreateMap<VehicleContract, VehicleContractDTO>();

            CreateMap<Rental, RentalDTO>().IncludeAllDerived();
            CreateMap<RentalDTO, Rental>().IncludeAllDerived();

            CreateMap<RentalCreationDTO, VehicleRental>();

            CreateMap<VehicleRental, VehicleRentalDTO>();

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
