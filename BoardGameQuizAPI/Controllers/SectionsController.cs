﻿using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SectionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _context.Sections.ToListAsync();
            return Ok(sections);
        }

        [HttpGet("set/{setId}")]
        public async Task<IActionResult> GetSectionsBySet(int setId)
        {
            var sections = await _context.Sections
                .Where(s => true)
                .Select(s => new
                {
                    s.SectionId,
                    s.SectionName,
                    IsCompleted = _context.UserProgresses
                        .Any(up => up.SetId == setId && up.SectionId == s.SectionId && up.IsCompleted)
                })
                .ToListAsync();

            if (sections == null || !sections.Any())
                return NotFound("No sections found.");

            return Ok(sections);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSection(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null)
                return NotFound("Section not found.");

            return Ok(section);
        }

        [HttpGet("set/{setId}/user/{userId}/role/{roleId}")]
        public async Task<IActionResult> GetSectionsWithProgress(int setId, int userId, int roleId)
        {
            var sections = await (from s in _context.Sections
                                  join up in _context.UserProgresses
                                      on new { s.SectionId, SetId = setId, UserId = userId, RoleId = roleId }
                                      equals new { up.SectionId, up.SetId, up.UserId, up.RoleId } into userProgresses
                                  from up in userProgresses.DefaultIfEmpty()
                                  select new
                                  {
                                      s.SectionId,
                                      s.SectionName,
                                      IsCompleted = up != null && up.IsCompleted
                                  })
                                  .ToListAsync();

            return Ok(sections);
        }

        [HttpGet("set/{setId}/user/{userId}/role/{roleId}/summary")]
        public async Task<IActionResult> GetCompletionStatusAndTotalScore(int setId, int userId, int roleId)
        {
            var userProgress = await _context.UserProgresses
                .Where(up => up.SetId == setId && up.UserId == userId && up.RoleId == roleId)
                .ToListAsync();

            // Check if all 8 sections are completed
            bool isAllSectionsCompleted = userProgress.Count == 8 && userProgress.All(up => up.IsCompleted);

            // Calculate total score for the 8 sections
            int totalScore = userProgress.Sum(up => up.ScoreObtained);

            return Ok(new
            {
                IsAllSectionsCompleted = isAllSectionsCompleted,
                TotalScore = totalScore
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddSections([FromBody] List<Section> sections)
        {
            if (sections == null || !sections.Any())
            {
                return BadRequest("No sections provided.");
            }

            _context.Sections.AddRange(sections);
            await _context.SaveChangesAsync();

            return Ok("Sections added successfully.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSections([FromBody] List<int> sectionIds)
        {
            if (sectionIds == null || !sectionIds.Any())
            {
                return BadRequest("No section IDs provided.");
            }

            // Fetch sections that match the given IDs
            var sectionsToDelete = _context.Sections.Where(s => sectionIds.Contains(s.SectionId)).ToList();

            if (!sectionsToDelete.Any())
            {
                return NotFound("No matching sections found.");
            }

            _context.Sections.RemoveRange(sectionsToDelete);
            await _context.SaveChangesAsync();

            return Ok("Sections deleted successfully.");
        }

    }
}
