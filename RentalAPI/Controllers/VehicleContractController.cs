using AutoMapper;
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
    [Route("VehiclesRentalContracts")]
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
        public async Task<ActionResult<IEnumerable<ContractDTO>>> Index()
        {
            var result = await _contractService.ListAsync();
            var resource = _mapper.Map<IEnumerable<Contract>, IEnumerable<VehicleContractDTO>>(result);
        
            return Ok(resource);
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
