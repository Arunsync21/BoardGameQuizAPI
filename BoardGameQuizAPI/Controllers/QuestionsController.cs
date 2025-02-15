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

        [HttpGet("set/{setId}/section/{sectionId}")]
        public async Task<IActionResult> GetQuestionsWithOptions(int setId, int sectionId)
        {
            var questions = await (from q in _context.Questions
                                   join o in _context.Options
                                       on q.QuestionId equals o.QuestionId into options
                                   where q.SectionId == sectionId && q.SetId == setId
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
        public async Task<IActionResult> AddQuestions([FromBody] List<QuestionDto> questionDtos)
        {
            if (questionDtos == null || !questionDtos.Any())
            {
                return BadRequest("No questions provided.");
            }

            try
            {
                var questions = questionDtos.Select(dto => new Question
                {
                    SectionId = dto.SectionId,
                    SetId = dto.SetId,
                    RoleId = dto.RoleId,
                    QuestionId = dto.QuestionId,
                    QuestionType = dto.QuestionType,
                    QuestionText = dto.QuestionText,
                    Passage = dto.Passage,
                    ImageUrl = dto.ImageUrl,
                    VideoUrl = dto.VideoUrl,
                    TimeLimit = dto.TimeLimit
                }).ToList();

                _context.Questions.AddRange(questions);
                await _context.SaveChangesAsync();

                return Ok("Questions added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the questions.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestions([FromBody] List<int> questionIds)
        {
            if (questionIds == null || !questionIds.Any())
            {
                return BadRequest("No set IDs provided.");
            }

            // Fetch the sets that match the given IDs
            var questionsToDelete = _context.Questions.Where(s => questionIds.Contains(s.QuestionId)).ToList();

            if (!questionsToDelete.Any())
            {
                return NotFound("No matching questions found.");
            }

            try
            {
                _context.Questions.RemoveRange(questionsToDelete);
                await _context.SaveChangesAsync();

                return Ok("Questions deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting questions.");
            }
        }
    }
}
