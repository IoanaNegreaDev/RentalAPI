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
        private readonly IVehicleRentalService _rentalService;
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;
        public RentalDamagesController(IRentalDamageService rentalDamageService,
                                        IVehicleRentalService rentalService,
                                        IContractService contractService,
                                        IMapper mapper)
        {
            _rentalDamageService = rentalDamageService;
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

            var rentalDamage = new RentalDamage
            {
                RentalId = newDamage.RentalId,
                Damage = new Damage
                {
                    RentableItemId = newDamage.RentalId,
                    DamageDescription = newDamage.DamageDescription,
                    DamageCost = newDamage.DamageCost
                },
            };

            var rentalDamageAddResult = await _rentalDamageService.AddAsync(rentalDamage);
            if (!rentalDamageAddResult.Success)
                return BadRequest(rentalDamageAddResult.Message);

            var rentalDamageDTOResult = _mapper.Map<RentalDamage, RentalDamageDTO>(rentalDamageAddResult._entity);

            return Ok(rentalDamageDTOResult);
        }
    }
}
