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
        private readonly IVehicleContractService _contractService;
        private readonly IClientService _clientService;

        private readonly IMapper _mapper;
        public VehiclesRentalContractsController(IVehicleContractService contractService,
                                                 IClientService clientService,
                                                 IMapper mapper)
        {
            _contractService = contractService;
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleContract>>> Index()
        {
            var result = await _contractService.ListAsync();
            var resource = _mapper.Map<IEnumerable<Contract>, IEnumerable<VehicleContractDTO>>(result);

            return Ok(resource);
        }

        [HttpPost("Client")]
        public async Task<IActionResult> CreateContractForClient(ClientCreationDTO clientDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();
           
            var dbClient = await _clientService.FindByNameAsync(clientDTO.Name);
            if (dbClient == null)
            {
                var client = _mapper.Map<ClientCreationDTO, Client>(clientDTO);
                var addClientResult = await _clientService.AddAsync(client);

                if (!addClientResult.Success)
                    return BadRequest(addClientResult.Message);

                dbClient = addClientResult._entity;
            }

            var contract = new VehicleContract
            {
                ClientId = dbClient.Id
            };
                
            var addContractResult = await _contractService.AddAsync(contract);

            if (!addContractResult.Success)
                return BadRequest(addContractResult.Message);

            var resource = _mapper.Map<VehicleContract, VehicleContractDTO>(addContractResult._entity);

            return Ok(resource);
        }
    }
}
