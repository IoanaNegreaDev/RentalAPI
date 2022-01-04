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
    [Route("api/vehiclerentals")]
    public class VehicleRentalsController : Controller
    {
        private readonly IVehicleRentalService _rentalService;
        private readonly IMapper _mapper;
        public VehicleRentalsController(IVehicleRentalService rentalService,
                                        IMapper mapper)
        {
            _rentalService = rentalService;
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

            var rental = _mapper.Map<RentalCreationDTO, VehicleRental>(newRental);

            var result = await _rentalService.AddAsync(rental);
            if (!result.Success)
                return BadRequest(result.Message);

            var resource = _mapper.Map<VehicleRental, VehicleRentalDTO>(result._entity);

            return Ok(resource);
        }
    }
}
