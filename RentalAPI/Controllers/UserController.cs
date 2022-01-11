using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Authentication;
using RentalAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
  /*  [ApiController]
    [Route("api/users")]
    public class AuthentificationController : Controller
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public AuthentificationController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;         
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<RentalUser>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<RentalUser>, IEnumerable<UserDTO>>(result);

            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<RentalUser>>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.FindByIdAsync(id);
            if (result== null)
                return NotFound();

            var resultDTO = _mapper.Map<RentalUser, UserDTO>(result);

            return Ok(resultDTO);
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserWithToken>> RegisterUser(UserCreationDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<UserCreationDTO, RentalUser>(userDTO);

            var result = await _service.AddUserWithTokenAsync(user);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result._entity);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserCreationDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<UserCreationDTO, RentalUser>(userDTO);

            var result = await _service.FindByUserNameAndPasswordAsync(user.UserName, user.Password);
            if (result == null)
                return NotFound("Invalid user name or password.");

            var refreshTokenResult = await _service.RefreshUserTokenAsync(result);
            if (!refreshTokenResult.Success)
                return NotFound("Failed to refresh token for user." + refreshTokenResult.Message);

            return Ok(refreshTokenResult._entity);
        }
    }*/
}
