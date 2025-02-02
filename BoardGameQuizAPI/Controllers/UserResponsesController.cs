using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserResponsesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserResponsesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("response")]
        public async Task<IActionResult> SubmitResponse([FromBody] UserResponseDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Response data is required.");
            }

            try
            {
                var response = new UserResponse
                {
                    UserId = dto.UserId,
                    QuestionId = dto.QuestionId,
                    OptionId = dto.OptionId,
                    ResponseTime = dto.ResponseTime,
                    IsCorrect = dto.IsCorrect,
                    AnsweredAt = dto.AnsweredAt
                };

                _context.UserResponses.Add(response);
                await _context.SaveChangesAsync();
                return Ok("Response submitted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the response.");
            }
        }
    }
}
