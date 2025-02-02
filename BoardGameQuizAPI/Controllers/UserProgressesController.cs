using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProgressController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserProgressController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("save-progress")]
        public async Task<IActionResult> SaveUserProgress([FromBody] UserProgressDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Response data is required.");
            }

            try
            {
                var userProgress = new UserProgress
                {
                    UserId = dto.UserId,
                    SetId = dto.SetId,
                    SectionId = dto.SectionId,
                    IsCompleted = dto.IsCompleted,
                    CompletedAt = dto.CompletedAt
                };

                _context.UserProgresses.Add(userProgress);
                await _context.SaveChangesAsync();
                return Ok(userProgress);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the progress.");
            }
        }
    }
}