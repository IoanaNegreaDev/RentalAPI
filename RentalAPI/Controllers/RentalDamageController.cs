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
    [Route("RentalDamages")]
    public class RentalDamagesController : Controller
    {
        private readonly IRentalDamageService _rentalDamageService;
        private readonly IDamageService _damageService;
        private readonly IVehicleRentalService _rentalService;
        private readonly IVehicleContractService _contractService;
        private readonly IMapper _mapper;
        public RentalDamagesController(IRentalDamageService rentalDamageService,
                                        IDamageService damageService,
                                        IVehicleRentalService rentalService,
                                        IVehicleContractService contractService,
                                        IMapper mapper)
        {
            _rentalDamageService = rentalDamageService;
            _damageService = damageService;
            _rentalService = rentalService;
            _contractService = contractService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalDamageDTO>>> Index()
        {
            var result = await _rentalDamageService.ListAsync();
            var resource = _mapper.Map<IEnumerable<RentalDamage>, IEnumerable<RentalDamageDTO>>(result);

            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RentalDamageCreationDTO newDamage)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var rental = await _rentalService.FindByIdAsync(newDamage.RentalId);
            if (rental== null)
                return BadRequest();

            var damage = new Damage
            {
                RentableItemId = newDamage.RentalId,
                DamageDescription = newDamage.DamageDescription,
                DamageCost = newDamage.DamageCost
            };
           
            var result = await _damageService.AddAsync(damage);
            if (!result.Success)
                return BadRequest(result.Message);

            var rentalDamage = new RentalDamage
            {
                RentalId = newDamage.RentalId,
                DamageId = result._entity.Id
            };

            var rentalDamageAddResult = await _rentalDamageService.AddAsync(rentalDamage);
            if (!rentalDamageAddResult.Success)
                return BadRequest(rentalDamageAddResult.Message);

            var updatedRental = await _rentalService.FindByIdAsync(newDamage.RentalId);
            updatedRental.DamagePrice = updatedRental.RentalDamages.Sum(item => item.Damage.DamageCost);
            
            var updatedRentalResponse = await _rentalService.UpdateAsync(updatedRental);
            if (!updatedRentalResponse.Success)
                return BadRequest(updatedRentalResponse.Message);

            var dbContract = await _contractService.FindByIdAsync(updatedRental.ContractId);
            dbContract.TotalDamagePriceInDefaultCurrency = dbContract.Rentals.Sum(item => item.DamagePrice);
            var updatedContract = await _contractService.UpdateAsync(dbContract);
            if (!updatedContract.Success)
                return BadRequest(updatedContract.Message);

            var rentalDamageDTOResult = _mapper.Map<RentalDamage, RentalDamageDTO>(rentalDamageAddResult._entity);

            return Ok(rentalDamageDTOResult);
        }
    }
}
