using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebElectroShop.Server.Core.DTO;
using WebElectroShop.Server.Core.Entities;
using WebElectroShop.Server.Data.Contexts;

namespace WebElectroShop.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ElectroShopDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ElectroShopDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return Unauthorized();

            var token = GenerateToken(user);
            return Ok(new { token, user.Username, user.Role });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                return BadRequest("Tên người dùng đã tồn tại");

            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = model.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Người dùng tạo ra");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Users
                .Select(u => new { u.Id, u.Username, u.Role, u.IsActive })
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string role)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Role = role;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] string token)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Username = payload.Email,
                    PasswordHash = "", // không cần
                    Role = "Customer"
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var jwt = GenerateToken(user);
            return Ok(new { token = jwt, user.Username, user.Role });
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
