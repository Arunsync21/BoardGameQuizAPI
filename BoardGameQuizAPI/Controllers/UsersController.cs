using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollUser([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Name == user.Name))
            {
                return BadRequest("User already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { UserId = user.UserId });
        }

        [HttpGet("check-login")]
        public async Task<IActionResult> CheckPreviousLogin([FromQuery] string email, [FromQuery] string mobileNumber, [FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(mobileNumber))
            {
                return BadRequest("Please provide either an email or a mobile number.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    (!string.IsNullOrEmpty(email) && u.Email == email) ||
                    (!string.IsNullOrEmpty(mobileNumber) && u.MobileNumber == mobileNumber));

            if (user == null)
            {
                return NotFound("User not found. You can proceed with enrollment.");
            }

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(mobileNumber) &&
                user.Email == email && user.MobileNumber == mobileNumber)
            {
                var userDetails = new
                {
                    user,
                    EnteredName = userName
                };
                return Ok(userDetails);
            }

            if ((!string.IsNullOrEmpty(email) && user.Email == email) ||
                (!string.IsNullOrEmpty(mobileNumber) && user.MobileNumber == mobileNumber))
            {
                string message = user.Email == email
                    ? "An account with the same email already exists. Please use a different email for enrollment."
                    : "An account with the same mobile number already exists. Please use a different mobile number for enrollment.";

                return Conflict(message);
            }

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] User updatedUser)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Role = updatedUser.Role; // Update role or other details
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}