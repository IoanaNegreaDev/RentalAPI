using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("VehicleRentals")]
    public class VehicleRentalsController : Controller
    {
        private readonly IVehicleRentalService _rentalService;
        private readonly IVehicleContractService _contractService;
        private readonly IRentableService _rentableService;

        private readonly IMapper _mapper;
        public VehicleRentalsController(IVehicleRentalService rentalService,
                                        IVehicleContractService contractService,
                                        IRentableService rentableService,
                                        IMapper mapper)
        {
            _rentalService = rentalService;
            _contractService = contractService;
            _rentableService = rentableService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleRental>>> Index()
        {
            var result = await _rentalService.ListAsync();
            var resource = _mapper.Map<IEnumerable<Rental>, IEnumerable<VehicleRentalDTO>>(result);

            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RentalCreationDTO newRental)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dbRentable = await _rentableService.FindByIdAsync(newRental.RentedItemId);
            if (dbRentable == null)
                return BadRequest("Failed to find rentable item in database.");

            var dbContract = await _contractService.FindByIdAsync(newRental.ContractId);
            if (dbContract == null)
                return BadRequest("Failed to find contract in database.");

            var rental = _mapper.Map<RentalCreationDTO, VehicleRental>(newRental);
            rental.FullTankPrice = dbRentable.PricePerDay * (newRental.EndDate - newRental.StartDate).TotalDays;
            rental.StatusId = 1;
            rental.FullTank = false;
            var result = await _rentalService.AddAsync(rental);
            if (!result.Success)
                return BadRequest(result.Message);

            var dbContractUpdated = await _contractService.FindByIdAsync(newRental.ContractId);
            var totalBasePrice = dbContractUpdated.Rentals.Sum(item => item.BasePrice);
            dbContractUpdated.TotalBasePriceInDefaultCurrency = totalBasePrice;

            var totalDdamagePrice = dbContractUpdated.Rentals.Sum(item => item.DamagePrice);
            dbContractUpdated.TotalDamagePriceInDefaultCurrency = totalDdamagePrice;

            var vehicleRentals = dbContractUpdated.Rentals.Cast<VehicleRental>();
            var totalFullTankPrice = vehicleRentals.Sum(item => item.FullTankPrice);

            dbContractUpdated.TotalFullTankPriceInDefaultCurrency = (float)totalFullTankPrice;

            var updateContractResult = await _contractService.UpdateAsync(dbContractUpdated);
            if (!updateContractResult.Success)
                return BadRequest(updateContractResult.Message);

            var resource = _mapper.Map<VehicleRental, VehicleRentalDTO>(result._entity);

            return Ok(resource);
        }
    }
}
