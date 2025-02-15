using BoardGameQuizAPI.Models;
using BoardGameQuizAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameQuizAPI.Controllers
{

    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public AuthController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Mobile))
            {
                return BadRequest("Email and Mobile are required.");
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Mobile == request.Mobile);

            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            var mobileExists = await _context.Users.AnyAsync(u => u.Mobile == request.Mobile);

            if (existingUser != null)
            {
                // Returning JWT token for existing user
                LoginResponseModel loginResponse = _authService.GenerateToken(existingUser);
                return Ok(loginResponse);
            }
            else if (emailExists || mobileExists)
            {
                if (emailExists)
                {
                    return BadRequest("An account with the same email already exists. Please use a different email for enrollment.");
                }
                else
                {
                    return BadRequest("An account with the same mobile number already exists. Please use a different mobile number for enrollment.");
                }
            }
            else
            {
                // Create a new user and return JWT token
                var newUser = new User
                {
                    Name = request.Name,
                    Designation = request.Designation,
                    Department = request.Department,
                    Mobile = request.Mobile,
                    Email = request.Email,
                    Title = request.Title,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                LoginResponseModel loginResponse = _authService.GenerateToken(newUser);
                return Ok(loginResponse);
            }
        }
    }
}
