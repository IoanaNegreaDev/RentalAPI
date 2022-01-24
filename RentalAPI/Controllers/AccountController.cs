using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
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

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserRegistrationDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var credentials = _mapper.Map<UserRegistrationDTO, UserCredentials>(userDTO);

            var result = await _service.RegisterAsync(credentials);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result._entity);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
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
