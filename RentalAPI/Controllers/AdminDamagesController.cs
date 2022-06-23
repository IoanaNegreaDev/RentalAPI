using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("/api/contracts/{contractId}/rentals/{rentalId}/damages")]
    public class DamagesController : Controller
    {
        private readonly IDamageService _service;
        private readonly IMapper _mapper;
        public DamagesController(IDamageService rentalDamageService,
                                 IMapper mapper)
        {
            _service = rentalDamageService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<DamageDTO>>> Get([FromRoute]int contractId, [FromRoute] int rentalId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            var result = await _service.ListAsync(contractId, rentalId);
            if (!result.Success)
                return BadRequest(result.Message);

            if (((ICollection<Damage>)result._entity).Count == 0)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Damage>, IEnumerable<DamageDTO>>(result._entity);

            return Ok(resultDTO);
        }


        [HttpGet("{damageId}")]
        [EnableQuery]
        public async Task<ActionResult<ContractDTO>> Get([FromRoute] int contractId, [FromRoute] int rentalId, [FromRoute] int damageId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            if (damageId <= 0)
                return BadRequest("damageId must be bigger than 0.");

            var result = await _service.FindByIdAsync(contractId, rentalId, damageId);
            if (!result.Success)
                return BadRequest(result.Message);

            if (result._entity == null)
                return NotFound();

            var resultDTO = _mapper.Map<Damage, DamageDTO>(result._entity);

            return Ok(resultDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromRoute] int contractId, [FromRoute] int rentalId, [FromBody] DamageCreationDTO newDamageDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            var newDamage = _mapper.Map<DamageCreationDTO, Damage>(newDamageDTO);

            var result = await _service.AddAsync(contractId, rentalId, newDamage);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Damage, DamageDTO>(result._entity);

            return CreatedAtAction(nameof(Get), 
                                    ControllerContext.RouteData.Values["controller"].ToString(), 
                                    new { id = resultDTO.Id }, 
                                    resultDTO);
        }

        [HttpDelete("{damageId}")]
        [EnableQuery]
        public async Task<IActionResult> Delete([FromRoute] int contractId, [FromRoute] int rentalId, [FromRoute] int damageId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (rentalId <= 0)
                return BadRequest("rentalId must be bigger than 0.");

            if (damageId <= 0)
                return BadRequest("damageId must be bigger than 0.");

            var result = await _service.DeleteAsync(contractId, rentalId, damageId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
