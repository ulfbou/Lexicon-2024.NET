using Microsoft.AspNetCore.Mvc;
using MessengerAPI.Authentication.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MessengerAPI.Authentication.Models;
using MessengerAPI.Authentication.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MessengerAPI.Authentication.Controllers
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

        [HttpPost("Register")]
        [AllowAnonymous]
        // Hash and Salt Passwords
        public async Task<ActionResult<User>> Register(User user)
        {
            if (await _unitsOfWork.Users.AnyAsync(u => u.Username == user.Username))
            {
                return Conflict();
            }

            user.Password = HashPassword(user.Password);
            _unitsOfWork.Users.Add(user);
            await _unitsOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Register), user);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Login(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            User? authenticatedUser = await AuthenticateUserAsync(user);

            if (authenticatedUser is null)
            {
                return Unauthorized();
            }

            return Ok(GenerateJsonWebToken(authenticatedUser));
        }

        private bool ValidatePassword(string password, string correctHash)
        {
            var parts = correctHash.Split('|');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var newHash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 16);

            return newHash.SequenceEqual(hash);
        }

        // Generate code that uses secure hash and salt methods
        private string HashPassword(string password)
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 16);
            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
        }

        private async Task<User?> AuthenticateUserAsync(User userInfo)
        {
            var user = (await _unitsOfWork.Users.FindAsync(u => u.Username == userInfo.Username))?.FirstOrDefault();

            if (user is null)
            {
                return null;
            }

            if (user.Id != userInfo.Id)
            {
                return null;
            }

            if (!ValidatePassword(userInfo.Password, user.Password))
            {
                return null;
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
