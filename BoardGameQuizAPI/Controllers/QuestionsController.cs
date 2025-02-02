using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("section/{sectionId}")]
        public async Task<IActionResult> GetQuestionsWithOptions(int sectionId)
        {
            var questions = await (from q in _context.Questions
                                   join o in _context.Options
                                       on q.QuestionId equals o.QuestionId into options
                                   where q.SectionId == sectionId
                                   select new
                                   {
                                       q.QuestionId,
                                       q.QuestionText,
                                       Options = options.Select(o => new
                                       {
                                           o.OptionId,
                                           o.OptionText
                                       }),
                                       CorrectOption = options
                                   .Where(o => o.IsCorrect) // Filter for the correct option
                                   .Select(o => new
                                   {
                                       o.OptionId,
                                       o.OptionText
                                   })
                                   .FirstOrDefault()
                                   })
                                  .ToListAsync();

            return Ok(questions);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDto dto)
        {
            if (dto == null)
            {
                return BadRequest("No question provided.");
            }

            try
            {
                var question = new Question
                {
                    SectionId = dto.SectionId,
                    QuestionId = dto.QuestionId,
                    QuestionType = dto.QuestionType,
                    QuestionText = dto.QuestionText,
                    Passage = dto.Passage,
                    ImageUrl = dto.ImageUrl,
                    VideoUrl = dto.VideoUrl,
                    TimeLimit = dto.TimeLimit
                };

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                return Ok("Question added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the question.");
            }
        }
    }
}
