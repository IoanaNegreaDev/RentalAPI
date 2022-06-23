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
    [Route("api/contracts")]
    public class AdminContractsController : Controller
    {
        private readonly IContractService _service;
        private readonly IMapper _mapper;
        public AdminContractsController(IContractService contractService,
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

            if (((ICollection < Contract>)result).Count == 0)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Contract>, IEnumerable<ContractDTO>>(result);

            return Ok(resultDTO);
        }

        [HttpGet("{contractId}")]
        [EnableQuery]
        public async Task<ActionResult<ContractDTO>> Get([FromRoute] int contractId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.FindByIdAsync(contractId);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Contract, ContractDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPut("{contractId}")]
        [EnableQuery]
        public async Task<IActionResult> Update([FromRoute] int contractId, [FromQuery] int paymentCurrencyId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            if (contractId <= 0)
                return BadRequest("paymentCurrencyId must be bigger than 0.");

            var result = await _service.UpdateAsync(contractId, paymentCurrencyId);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Contract, ContractDTO>(result._entity);

            return Ok(resultDTO);
        }

        [HttpDelete("{contractId}")]
        [EnableQuery]
        public async Task<IActionResult> Delete([FromRoute] int contractId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("id must be bigger than 0.");

            var result = await _service.DeleteAsync(contractId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}