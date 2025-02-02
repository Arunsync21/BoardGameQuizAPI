using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OptionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOptions(int id)
        {
            var options = await _context.Options
                .Where(o => o.QuestionId == id) // Filter by QuestionId
                .ToListAsync();

            if (options == null || !options.Any())
            {
                return NotFound("No options found for the given question ID.");
            }

            return Ok(options);
        }


        [HttpPost("option")]
        public async Task<IActionResult> AddOptions([FromBody] List<OptionDto> dto)
        {
            if (dto == null || !dto.Any())
            {
                return BadRequest("Response data is required.");
            }

            try
            {
                var options = dto.Select(o => new Option
                {
                    QuestionId = o.QuestionId,
                    OptionText = o.OptionText,
                    IsCorrect = o.IsCorrect
                }).ToList();

                _context.Options.AddRange(options);
                await _context.SaveChangesAsync();
                return Ok("Options submitted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the options.");
            }
        }
    }
}
