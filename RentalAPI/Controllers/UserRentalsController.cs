using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, NormalUser")]
    [ApiController]
    [Route("api/users/contracts/{contractId}/rentals/vehiclerentals")]
    public class UserRentalsController : Controller
    {
        private readonly IVehicleRentalService _vehicleRentalService;
        private readonly IMapper _mapper;
        public UserRentalsController(IVehicleRentalService vehicleRentalService,
                                 IMapper mapper)
        {
            _vehicleRentalService = vehicleRentalService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<VehicleRental>>> Get([FromRoute] int contractId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            var authenticatedUserId = HttpContext.User.Identity.Name;

            var rentals = await _vehicleRentalService.ListAsync(authenticatedUserId, contractId);
            if (!rentals.Success)
                return BadRequest(rentals.Message);

            if (rentals._entity == null || ((ICollection<VehicleRental>)rentals._entity).Count == 0)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rental>, IEnumerable<RentalDTO>>(rentals._entity);

            return Ok(resultDTO);
        }

        [HttpGet("{rentalId}")]
        [EnableQuery]
        public async Task<ActionResult<VehicleRental>> Get([FromRoute] int contractId, [FromRoute] int rentalId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            var rental = await _vehicleRentalService.FindByIdAsync(contractId, rentalId);
            if (!rental.Success)
                return BadRequest(rental.Message);

            if (rental._entity == null)
                return NotFound();

            var resultDTO = _mapper.Map<Rental, RentalDTO>(rental._entity);

            return Ok(resultDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromRoute] int contractId, RentalCreationDTO newRental)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            var rental = _mapper.Map<RentalCreationDTO, VehicleRental>(newRental);

            var result = await _vehicleRentalService.AddAsync(contractId, rental);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Rental, RentalDTO>(result._entity);

            return CreatedAtAction(nameof(Get),
                        ControllerContext.RouteData.Values["controller"].ToString(),
                        new { id = resultDTO.Id },
                        resultDTO);
        }

        [HttpPut("{rentalId}")]
        [EnableQuery]
        public async Task<IActionResult> Update([FromRoute] int contractId, [FromRoute] int rentalId, [FromBody] VehicleRentalUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            var rental = _mapper.Map<VehicleRentalUpdateDTO, VehicleRental>(updateDTO);
            rental.Id = rentalId;
            rental.ContractId = contractId;

            var result = await _vehicleRentalService.UpdateAsync(contractId, rental);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Rental, RentalDTO>(result._entity);

            return Ok(resultDTO);
        }

        [HttpDelete("{rentalId}")]
        [EnableQuery]
        public async Task<IActionResult> Delete([FromRoute] int contractId, [FromRoute] int rentalId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            var result = await _vehicleRentalService.DeleteAsync(contractId, rentalId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
