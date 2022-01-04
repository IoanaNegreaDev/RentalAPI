using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
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
    [Route("api/clients")]
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        public ClientsController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> Get()
        {
            var result =  await _clientService.ListAsync();
            var resource = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDTO>>(result);

            return Ok(resource);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> Get(int id)
        {
            var result = await _clientService.FindByIdAsync(id);
            var resource = _mapper.Map<Client, ClientDTO>(result);

            return Ok(resource);
        }

        [HttpPut]
        [EnableQuery]
        public async Task<IActionResult> Update(int Id, ClientUpdateDTO clientDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = _mapper.Map<ClientUpdateDTO, Client>(clientDTO);
            client.Id = Id;
            var updateClientResult = await _clientService.UpdateAsync(client);

            if (!updateClientResult.Success)
                return BadRequest(updateClientResult.Message);

            var resource = _mapper.Map<Client, ClientDTO>(updateClientResult._entity);

            return Ok(resource);
        }
    }
}
