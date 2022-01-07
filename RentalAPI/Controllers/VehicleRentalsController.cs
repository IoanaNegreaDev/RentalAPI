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
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<VehicleRental>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _rentalService.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rental>, IEnumerable<VehicleRentalDTO>>(result);

            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<VehicleRental>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _rentalService.FindByIdAsync(id);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Rental, VehicleRentalDTO>(result);

            return Ok(resultDTO);
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

            var resultDTO = _mapper.Map<VehicleRental, VehicleRentalDTO>(result._entity);

            return CreatedAtAction(nameof(Get),
                        ControllerContext.RouteData.Values["controller"].ToString(),
                        new { id = resultDTO.Id },
                        resultDTO);
        }

        [HttpPut]
        [EnableQuery]
        [BasicAuthorization]
        public async Task<IActionResult> Update(int id, VehicleRentalUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id <= 0)
                return BadRequest("id must be bigger than 0.");

            var rental = _mapper.Map<VehicleRentalUpdateDTO, VehicleRental>(updateDTO);
            rental.Id = id;

            var result = await _rentalService.UpdateAsync(rental);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<VehicleRental, VehicleRentalDTO>(result._entity);

            return Ok(resultDTO);
        }

        [HttpDelete]
        [EnableQuery]
        [BasicAuthorization]
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
