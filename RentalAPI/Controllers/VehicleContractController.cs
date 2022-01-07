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
    [Route("api/vehiclerentalcontracts")]
    public class VehiclesRentalContractsController : Controller
    {
        private readonly IContractService _service;
        private readonly IMapper _mapper;
        public VehiclesRentalContractsController(IContractService contractService,
                                                 IClientService clientService,
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

            var resultDTO = _mapper.Map<IEnumerable<Contract>, IEnumerable<VehicleContractDTO>>(result);
        
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

            var resultDTO = _mapper.Map<Contract, VehicleContractDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddContract(ContractCreationDTO contractDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var contract = _mapper.Map<ContractCreationDTO, Contract>(contractDTO);

            var result = await _service.AddAsync(contract);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Contract, VehicleContractDTO>(result._entity);

            return CreatedAtAction(nameof(Get),
                        ControllerContext.RouteData.Values["controller"].ToString(),
                        new { id = resultDTO.Id },
                        resultDTO);
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

            var result = await _service.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
