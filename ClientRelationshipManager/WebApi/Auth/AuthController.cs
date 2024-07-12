using Application.DTOs;
using Application.Services;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly JwtTokenService _jwtTokenService;

        
        public AuthController(IUserAccountService userAccountService, JwtTokenService jwtTokenService)
        {
            _userAccountService = userAccountService;
            _jwtTokenService = jwtTokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userAccountService.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest("Podano błędny login lub hasło");

            var token = _jwtTokenService.GenerateJwtToken(user);
            //return Ok(token);
            return Ok(new
            {
                User = user, 
                Token = token
            });
        }

    }
}
