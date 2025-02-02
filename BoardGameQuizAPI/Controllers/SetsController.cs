using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SetsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSets()
        {
            var sets = await _context.Sets.ToListAsync();
            return Ok(sets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSet(int id)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
                return NotFound("Set not found.");

            return Ok(set);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSetsWithProgress(int userId)
        {
            var sets = await (from s in _context.Sets
                              join up in _context.UserProgresses
                                  on new { s.SetId, UserId = userId } equals new { up.SetId, up.UserId } into userProgresses
                              from up in userProgresses.DefaultIfEmpty()
                              select new
                              {
                                  s.SetId,
                                  s.SetName,
                                  IsCompleted = up != null && up.IsCompleted
                              })
                      .Distinct()
                      .ToListAsync();

            return Ok(sets);
        }

        [HttpPost]
        public async Task<IActionResult> AddSets([FromBody] List<Set> sets)
        {
            if (sets == null || !sets.Any())
            {
                return BadRequest("No sets provided.");
            }

            try
            {
                _context.Sets.AddRange(sets);
                await _context.SaveChangesAsync();
                return Ok("Sets added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding sets.");
            }
        }
    }
}
