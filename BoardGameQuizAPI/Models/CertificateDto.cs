namespace BoardGameQuizAPI.Models
{
    public class CertificateDto
    {
        public int CertificateId { get; set; }
        public int UserId { get; set; }
        public int SetId { get; set; }
        public string CertificateType { get; set; }
        public int MarksObtained { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    }
}
