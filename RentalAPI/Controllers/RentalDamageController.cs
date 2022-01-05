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
    [Route("api/rentaldamages")]
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
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<RentalDamageDTO>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _rentalDamageService.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<RentalDamage>, IEnumerable<RentalDamageDTO>>(result);

            return Ok(resultDTO);
        }


        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<ContractDTO>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _rentalDamageService.FindByIdAsync(id);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<RentalDamage, RentalDamageDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RentalDamageCreationDTO newDamageDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newDamage = _mapper.Map<RentalDamageCreationDTO, RentalDamage>(newDamageDTO);

            var result = await _rentalDamageService.AddAsync(newDamage);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<RentalDamage, RentalDamageDTO>(result._entity);

            return CreatedAtAction(nameof(Get), 
                                    ControllerContext.RouteData.Values["controller"].ToString(), 
                                    new { id = resultDTO.Id }, 
                                    resultDTO);
        }
    }
}
