using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("api/vehiclerentalcontracts")]
    public class VehiclesRentalContractsController : Controller
    {
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;
        public VehiclesRentalContractsController(IContractService contractService,
                                                 IClientService clientService,
                                                 ICurrencyService currencyService,
                                                 IMapper mapper)
        {
            _contractService = contractService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ContractDTO>>> Get()
        {
            var result = await _contractService.ListAsync();
            var resultDTO = _mapper.Map<IEnumerable<Contract>, IEnumerable<VehicleContractDTO>>(result);
        
            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<ContractDTO>> Get(int id)
        {
            var result = await _contractService.FindByIdAsync(id);
            var resultDTO = _mapper.Map<Contract, VehicleContractDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddContract(ContractCreationDTO contractDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newContract = _mapper.Map<ContractCreationDTO, Contract>(contractDTO);

            var addContractResult = await _contractService.AddAsync(newContract);

            if (!addContractResult.Success)
                return BadRequest(addContractResult.Message);

            var resource = _mapper.Map<Contract, VehicleContractDTO>(addContractResult._entity);

            return CreatedAtAction(nameof(AddContract), new { id = resource.Id }, resource);
        }
    }
}
