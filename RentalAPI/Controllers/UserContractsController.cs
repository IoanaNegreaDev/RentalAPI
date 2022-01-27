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
    [Route("api/users/contracts")]
    public class UserContractsController : Controller
    {
        private readonly IContractService _service;
        private readonly IMapper _mapper;
        public UserContractsController(IContractService contractService,
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

            var authenticatedUserId = HttpContext.User.Identity.Name;

            var result = await _service.ListAsync(authenticatedUserId);

            if (!result.Success)
                return BadRequest(result.Message);

            if (result._entity == null)
                return NoContent();

            if (((ICollection<Contract>)result._entity).Count == 0)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Contract>, IEnumerable<ContractDTO>>(result._entity);
        
            return Ok(resultDTO);
        }

        [HttpGet("{contractId}", Name = "GetContractById")]
        [EnableQuery]
        public async Task<ActionResult<ContractDTO>> Get(int contractId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (contractId <= 0)
                return BadRequest("contractId must be bigger than 0.");

            var authenticatedUserId = HttpContext.User.Identity.Name;

            var result = await _service.FindByIdAsync(authenticatedUserId, contractId);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Contract, ContractDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddContract(int paymentCurrencyId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (paymentCurrencyId <= 0)
                return BadRequest("paymentCurrencyId should be bigger than 0.");

            var authenticatedUserId = HttpContext.User.Identity.Name;

            var result = await _service.AddAsync(authenticatedUserId, paymentCurrencyId);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Contract, ContractDTO>(result._entity);

            return CreatedAtRoute("GetContractById",
                                  new { id = resultDTO.Id },
                                  resultDTO);
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

            var authenticatedUserId = HttpContext.User.Identity.Name;

            var result = await _service.UpdateAsync(authenticatedUserId, contractId, paymentCurrencyId);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Contract, ContractDTO>(result._entity);

            return Ok(resultDTO);
        }
    }
}
