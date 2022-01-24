using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("api/contracts/{contractId}/rentals")]
    public class RentalsController : Controller
    {
        private readonly IRentalService _rentalService;
        private readonly IMapper _mapper;
        public RentalsController(IRentalService rentalService,
                                        IMapper mapper)
        {
            _rentalService = rentalService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Rental>>> Get(int contractId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            var rentals = await _rentalService.ListAsync(contractId);
            if (!rentals.Success)
                return BadRequest(rentals.Message);

            if (rentals._entity == null || ((ICollection<Rental>)rentals._entity).Count == 0)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rental>, IEnumerable<RentalDTO>>(rentals._entity);

            return Ok(resultDTO);
        }

        [HttpGet("{rentalId}")]
        [EnableQuery]
        public async Task<ActionResult<Rental>> Get(int contractId, int rentalId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            var rental = await _rentalService.FindByIdAsync(contractId, rentalId);
            if (!rental.Success)
                return BadRequest(rental.Message);

            if (rental._entity == null)
                return NotFound();

            var resultDTO = _mapper.Map<Rental, RentalDTO>(rental._entity);

            return Ok(resultDTO);
        }


        [HttpDelete("{rentalId}")]
        [EnableQuery]
        public async Task<IActionResult> Delete(int contractId, int rentalId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            var result = await _rentalService.DeleteAsync(contractId, rentalId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
