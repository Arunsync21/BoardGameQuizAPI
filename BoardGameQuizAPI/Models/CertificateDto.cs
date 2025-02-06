namespace BoardGameQuizAPI.Models
{
    public class CertificateDto
    {
        public int UserId { get; set; }
        public int SetId { get; set; }
        public string CertificateType { get; set; }
        public int MarksObtained { get; set; }
    }
}
