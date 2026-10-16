using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayContext _db;
        public AuthController(RaceDayContext db) { _db = db; }

        public record RegisterRequest(string FullName, string Email, string Password, string Role, string PhoneNumber);
        public record LoginRequest(string Email, string Password);

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            if (req.Role != "Organiser" && req.Role != "Participant")
                return BadRequest("Role must be Organiser or Participant");
            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                return BadRequest("Email already in use");

            var user = new User
            {
                FullName = req.FullName,
                Email = req.Email,
                PasswordHash = Hash(req.Password),
                Role = req.Role,
                PhoneNumber = req.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return StatusCode(201, new { user.UserId, user.Role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var hash = Hash(req.Password);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email && u.PasswordHash == hash);
            if (user == null) return Unauthorized();
            return Ok(new { user.UserId, user.Role });
        }

        public static string Hash(string password) =>
            Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
    }
}