using Microsoft.AspNetCore.Mvc;
using MessengerAPI.Data;
using MessengerAPI.Models;
using MessengerAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUoW _unitsOfWork;
        private readonly IConfiguration _configuration;

        public UsersController(IUoW unitsOfWork, IConfiguration configuration)
        {
            _unitsOfWork = unitsOfWork;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {


            if (await _unitsOfWork.Users.AnyAsync(user.Id))
            {
                return Conflict();
            }

            _unitsOfWork.Users.Add(user);
            await _unitsOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Register), user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            User userInfo = await _unitsOfWork.Users.GetAsync(user.Id);

            if (userInfo is null)
            {
                return Unauthorized();
            }

            return Ok(userInfo);
        }

        private User? AuthenticateUser(User userInfo)
        {
            User? user = null;

            if (userInfo.Username.ToLower() == "noname")
            {
                user = new User
                {
                    Username = "NoName",
                    Password = "123456",
                    FirstName = "No",
                    LastName = "Name",
                    Email = "",
                    Phone = "",
                    Created = DateTime.Now,
                    Updated = DateTime.Now
                };
            }

            return user;
        }

        private string GenerateJsonWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim (JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim (JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim ("DateOfJoining",userInfo.Created.ToString("yyyy-MM-dd")),
                new Claim (JwtRegisteredClaimNames.FamilyName, userInfo.LastName),
                new Claim (JwtRegisteredClaimNames.GivenName, userInfo.FirstName),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );

            return "";
        }
    }
}
