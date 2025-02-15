namespace BoardGameQuizAPI.Models
{
    public class UserProgressDto
    {
        public int ProgressId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int SetId { get; set; }
        public int SectionId { get; set; }
        public bool IsCompleted { get; set; }
        public int ScoreObtained { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
