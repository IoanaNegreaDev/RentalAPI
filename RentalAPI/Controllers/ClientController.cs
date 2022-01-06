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

        //  [BasicAuthorization]
        [Authorize]
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result =  await _clientService.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientDTO>>(result);

            return Ok(resultDTO);
        }

      //  [BasicAuthorization]
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<ClientDTO>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id <= 0)
                return BadRequest("id must be bigger than 0.");

            var result = await _clientService.FindByIdAsync(id);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Client, ClientDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPut]
        [EnableQuery]
        public async Task<IActionResult> Update(int id, ClientUpdateDTO clientDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id <= 0)
                return BadRequest("id must be bigger than 0.");

            var client = _mapper.Map<ClientUpdateDTO, Client>(clientDTO);
            client.Id = id;

            var result = await _clientService.UpdateAsync(client);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<Client, ClientDTO>(result._entity);

            return Ok(resultDTO);
        }
    }
}
