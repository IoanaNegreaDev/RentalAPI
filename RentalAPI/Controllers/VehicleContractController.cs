using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("VehiclesRentalContracts")]
    public class VehiclesRentalContractsController : Controller
    {
        private readonly IContractService _contractService;
        private readonly ICurrencyService _currencyService;
        private readonly IClientService _clientService;

        private readonly IMapper _mapper;
        public VehiclesRentalContractsController(IContractService contractService,
                                                 IClientService clientService,
                                                 ICurrencyService currencyService,
                                                 IMapper mapper)
        {
            _contractService = contractService;
            _clientService = clientService;
            _currencyService =currencyService;
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
        public async Task<IActionResult> CreateContractForClient(ContractCreationDTO contractDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dbCurrency = await _currencyService.FindByNameAsync(contractDTO.PaymentCurrency);
            if (dbCurrency == null)
                return BadRequest("Currency not supported.");


            var dbClient = await _clientService.FindByNameAsync(contractDTO.Name);
            if (dbClient == null)
            {
                dbClient = new Client
                {
                    Name = contractDTO.Name,
                    Mobile = contractDTO.Mobile,
                };
            }
            
            var newcontract = new Contract
            {
                PaymentCurrencyId = dbCurrency.Id,
                Client = dbClient
            };
             
            var addContractResult = await _contractService.AddAsync(newcontract);

            if (!addContractResult.Success)
                return BadRequest(addContractResult.Message);

            var resource = _mapper.Map<Contract, VehicleContractDTO>(addContractResult._entity);

            return Ok(resource);
        }
    }
}
