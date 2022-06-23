using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/users")]
    public class AccountController : Controller
    { 
        private readonly IAccountService _service;
        private readonly IMapper _mapper;
        public AccountController(IAccountService service,
                                 IMapper mapper)
                                                                
        {
            _service = service;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegistrationDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var credentials = _mapper.Map<UserRegistrationDTO, UserCredentials>(userDTO);

            var result = await _service.RegisterAsync(credentials);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result._entity);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var credentials = _mapper.Map<UserLoginDTO, UserCredentials>(userDTO);

            var userWithToken = await _service.LoginAsync(credentials);
            if (!userWithToken.Success)
                return NotFound("Failed to login." + userWithToken.Message);

            if (Request.Query.Keys.Contains("ReturnUrl"))
                return Redirect(Request.Query["ReturnUrl"].First());

            return Ok(userWithToken._entity);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            await _service.LogoutAsync(refreshToken);
            
            return Ok();
        }
        
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = HttpContext.Request.Cookies["refreshToken"];

            var userWithToken = await _service.RefreshTokensAsync(token);
            if (!userWithToken.Success)
                return BadRequest();

            return Ok(userWithToken._entity);
        }
    }
}
