
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayContext _context;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthController(RaceDayContext context)
        {
            _context = context;
        }

        // Registration request
        public class RegisterRequest
        {
            [Required]
            public string FirstName { get; set; } = "";

            [Required]
            public string LastName { get; set; } = "";

            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            [MinLength(8)]
            public string Password { get; set; } = "";

            public string Role { get; set; } = "Participant";
        }

        // Login request
        public class LoginRequest
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            public string Password { get; set; } = "";
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request)
        {
            // Public registration cannot create Organisers.
            if (request.Role != "Participant")
            {
                return BadRequest(
                    "Public registration is for Participants only.");
            }

            var email = request.Email.Trim().ToLowerInvariant();

            if (await _context.Users.AnyAsync(
                u => u.Email == email))
            {
                return BadRequest("Email already registered.");
            }

            var user = new User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = email,
                Role = "Participant",
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash =
                _hasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Registration successful",
                user.UserID,
                user.Email,
                user.Role
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return Unauthorized(
                    "Invalid email or password.");
            }

            var result = _hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(
                    "Invalid email or password.");
            }

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash =
                    _hasher.HashPassword(user, request.Password);

                await _context.SaveChangesAsync();
            }

            HttpContext.Session.Clear();

            HttpContext.Session.SetInt32(
                "UserID", user.UserID);

            HttpContext.Session.SetString(
                "Role", user.Role);

            return Ok(new
            {
                Message = "Login successful",
                user.UserID,
                user.Email,
                user.Role
            });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Ok(new
            {
                Message = "Logged out successfully"
            });
        }

        // GET: api/auth/me
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return Unauthorized("Please log in.");
            }

            return Ok(new
            {
                UserID = userId.Value,
                Role = HttpContext.Session.GetString("Role")
            });
        }
    }
}
