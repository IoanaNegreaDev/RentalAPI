using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Persistance;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("Clients")]
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        public ClientsController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        // GET: Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> Index()
        {
            var result =  await _clientService.ListAsync();
            var resource = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDTO>>(result);

            return Ok(resource);
        }

        // POST: Clients
    /*    [HttpPost("Add")]
        public async Task<IActionResult> Add(ClientDTO newClient)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = _mapper.Map<ClientDTO, Client>(newClient);
            var result = await _clientService.AddAsync(client);

            if (!result.Success)
                return BadRequest(result.Message);

            var resource = _mapper.Map<Client, ClientDTO>(result._entity);

            return Ok(resource);
        }*/

        // POST: Clients
        [HttpPut]//("Update")]
        public async Task<IActionResult> Update(ClientDTO clientDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = _mapper.Map<ClientDTO, Client>(clientDTO);
            var result = await _clientService.AddAsync(client);

            if (!result.Success)
                return BadRequest(result.Message);

            var resource = _mapper.Map<Client, ClientDTO>(result._entity);

            return Ok(resource);
        }
    }
}
