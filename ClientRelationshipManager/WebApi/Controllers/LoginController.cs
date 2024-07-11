using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using BusinessLogic.Models;
using DataAccess.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserAccountRepository _repository;

        public LoginController(IConfiguration config , UserAccountRepository repository)
        {
            _config = config;
            _repository = repository;
        }


        //[AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> LoginDB(LoginModel model)
        {
            var mappedUser = new UserAccount()
            {
                Name = model.Username,
                Password = model.Password,
               
            };

            var user = await AuthenticateDB(mappedUser); 
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return BadRequest("Podano błędy login lub hasło");
        }


        //To authenticate user

        private async Task<UserAccount> AuthenticateDB(UserAccount userAccount)
        {
            var users = await _repository.GetAllAsync();
            var user = users.SingleOrDefault(x => x.Name == userAccount.Name && x.Password == userAccount.Password);
            //if (user != null)
            //{
            //    var lModel = new UserAccount()
            //    {
            //        Name = user.Name,
            //        Password = user.Password
            //    };
            //    return lModel;
            //}
            //return null;
            return user;
        }



        // To generate token 
        private string GenerateToken(UserAccount user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Name),

            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(5),                                   //token expiry date
                signingCredentials: credentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }

}
