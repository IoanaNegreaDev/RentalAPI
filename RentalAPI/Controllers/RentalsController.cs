using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Authentication;
using RentalAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("api/rentals")]

    public class RentalsController : Controller
    {
        private readonly IRentalService _rentalService;
        private readonly IVehicleRentalService _vehicleRentalService;
        private readonly IMapper _mapper;
        public RentalsController(IRentalService rentalService,
                                        IVehicleRentalService vehicleRentalService,
                                        IMapper mapper)
        {
            _rentalService = rentalService;
            _vehicleRentalService = vehicleRentalService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Rental>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _rentalService.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rental>, IEnumerable<RentalDTO>>(result);

            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<Rental>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _rentalService.FindByIdAsync(id);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Rental, RentalDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPost("vehicle")]
        public async Task<IActionResult> Add(RentalCreationDTO newRental)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var rental = _mapper.Map<RentalCreationDTO, VehicleRental>(newRental);

            var result = await _vehicleRentalService.AddAsync(rental);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Rental, RentalDTO>(result._entity);

            return CreatedAtAction(nameof(Get),
                        ControllerContext.RouteData.Values["controller"].ToString(),
                        new { id = resultDTO.Id },
                        resultDTO);
        }

        [HttpPut("vehicle")]
        [EnableQuery]
        public async Task<IActionResult> Update(int id, VehicleRentalUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id <= 0)
                return BadRequest("id must be bigger than 0.");

            var rental = _mapper.Map<VehicleRentalUpdateDTO, VehicleRental>(updateDTO);
            rental.Id = id;

            var result = await _vehicleRentalService.UpdateAsync(rental);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Rental, RentalDTO>(result._entity);

            return Ok(resultDTO);
        }

        [HttpDelete]
        [EnableQuery]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id <= 0)
                return BadRequest("id must be bigger than 0.");

            var result = await _rentalService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
