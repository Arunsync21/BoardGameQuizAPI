namespace BoardGameQuizAPI.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SetId { get; set; }
        public Set Set { get; set; }
        public string CertificateType { get; set; } // Gold, Silver, Bronze
        public int MarksObtained { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    }
}
