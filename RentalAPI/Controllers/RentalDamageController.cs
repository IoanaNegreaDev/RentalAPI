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
        private readonly IMapper _mapper;
        public RentalDamagesController(IRentalDamageService rentalDamageService,
                                        IMapper mapper)
        {
            _rentalDamageService = rentalDamageService;
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
        public async Task<IActionResult> Add(RentalDamageCreationDTO newDamageDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newDamage = _mapper.Map<RentalDamageCreationDTO, RentalDamage>(newDamageDTO);

            var addRentalDamageResult = await _rentalDamageService.AddAsync(newDamage);
            if (!addRentalDamageResult.Success)
                return BadRequest(addRentalDamageResult.Message);

            var rentalDamageDTOResult = _mapper.Map<RentalDamage, RentalDamageDTO>(addRentalDamageResult._entity);

            return Ok(rentalDamageDTOResult);
        }
    }
}
