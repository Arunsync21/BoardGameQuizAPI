using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CertificatesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCertificate([FromBody] Certificate certificate)
        {
            if (certificate == null)
                return BadRequest("Certificate data is required.");

            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            return Ok("Certificate generated successfully.");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCertificates(int userId)
        {
            var certificates = await _context.Certificates
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return Ok(certificates);
        }
    }
}
