using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/contracts")]
   
    public class VehiclesRentalContractsController : Controller
    {
        private readonly IContractService _service;
        private readonly IMapper _mapper;
        public VehiclesRentalContractsController(IContractService contractService,
                                                 ICurrencyService currencyService,
                                                 IMapper mapper)
        {
            _service = contractService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ContractDTO>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Contract>, IEnumerable<ContractDTO>>(result);
        
            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<ContractDTO>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.FindByIdAsync(id);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Contract, ContractDTO>(result);

            return Ok(resultDTO);
        }

        [Authorize]
        [HttpPost("VehicleAut")]
        public async Task<IActionResult> AddContract(int PaymentCurrencyId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.AddAsync(User.Identity.Name, PaymentCurrencyId);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Contract, ContractDTO>(result._entity);

            return CreatedAtAction(nameof(Get),
                        ControllerContext.RouteData.Values["controller"].ToString(),
                        new { id = resultDTO.Id },
                        resultDTO);
        }

        [HttpDelete]
        [EnableQuery]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id <= 0)
                return BadRequest("id must be bigger than 0.");

            var result = await _service.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
