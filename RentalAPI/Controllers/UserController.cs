using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
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
/*    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(result);

            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<User>>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.FindByIdAsync(id);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<User, UserDTO>(result);

            return Ok(resultDTO);
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            string userName = HttpContext.User.Identity.Name;
            var result = await _service.FindByUserNameAsync(userName);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<User, UserDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserCreationDTO newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<UserCreationDTO, User>(newUser);

            var result = await _service.AddAsync(user);
            if (!result.Success)
                return BadRequest(result.Message);

            var resultDTO = _mapper.Map<User, UserDTO>(result._entity);

            return CreatedAtAction(nameof(Get),
                        ControllerContext.RouteData.Values["controller"].ToString(),
                        new { id = resultDTO.Id },
                        resultDTO);
        }
    }*/
}
