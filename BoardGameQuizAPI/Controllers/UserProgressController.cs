using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    RoleId = dto.RoleId,
                    SetId = dto.SetId,
                    SectionId = dto.SectionId,
                    IsCompleted = dto.IsCompleted,
                    ScoreObtained = dto.ScoreObtained
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

        [HttpDelete("delete-responses")]
        public async Task<IActionResult> DeleteResponses([FromBody] List<int> responseIds)
        {
            if (responseIds == null || !responseIds.Any())
            {
                return BadRequest("No ResponseIds provided.");
            }

            var responsesToDelete = await _context.UserProgresses
                .Where(up => responseIds.Contains(up.ProgressId))
                .ToListAsync();

            if (!responsesToDelete.Any())
            {
                return NotFound("No matching responses found.");
            }

            _context.UserProgresses.RemoveRange(responsesToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Responses deleted successfully.", deletedCount = responsesToDelete.Count });
        }

    }
}