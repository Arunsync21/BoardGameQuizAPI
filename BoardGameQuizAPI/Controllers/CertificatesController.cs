using BoardGameQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Xml.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Net.Mail;
using System.Net;
using BoardGameQuizAPI.Services;

namespace BoardGameQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CertificateService _certificateService;


        public CertificatesController(AppDbContext context, CertificateService certificateService)
        {
            _context = context;
            _certificateService = certificateService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCertificate([FromBody] CertificateDto certificatedto)
        {
            User currentUser = await _context.Users.FindAsync(certificatedto.UserId);
            var certificate = new Certificate()
            {
                UserId = certificatedto.UserId,
                SetId = certificatedto.SetId,
                CertificateType = certificatedto.CertificateType,
                MarksObtained = certificatedto.MarksObtained,
                IssuedAt = DateTime.UtcNow,
            };
            string level = string.Empty;
            double percentage = (double)certificatedto.MarksObtained / 400 * 100;

            if (percentage >= 80)
                level = "Gold";
            else if (percentage >= 70)
                level = "Silver";
            else if(percentage >= 60)
                level = "Bronze";
            
            if (currentUser != null)
            {

                byte[] pdfData = _certificateService.GenerateCertificate(currentUser.Name, currentUser.Designation, currentUser.Department, level);
                if (pdfData == null || pdfData.Length == 0)
                {
                    return BadRequest("Failed to generate certificate.");
                }

                return File(pdfData, "application/pdf", $"{level}_Certificate.pdf");
            }

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
