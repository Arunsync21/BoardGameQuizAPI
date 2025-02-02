using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProgressController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}/{setId}")]
        public async Task<IActionResult> GetProgress(int userId, int setId)
        {
            var progress = await _context.UserProgresses
                .Where(up => up.UserId == userId && up.SetId == setId)
                .ToListAsync();

            return Ok(progress);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> MarkSectionCompleted([FromBody] UserProgress progress)
        {
            if (progress == null)
                return BadRequest("Progress data is required.");

            _context.UserProgresses.Add(progress);
            await _context.SaveChangesAsync();

            return Ok("Section marked as completed.");
        }
    }
}
