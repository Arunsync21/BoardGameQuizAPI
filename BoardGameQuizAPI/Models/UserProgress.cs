namespace BoardGameQuizAPI.Models
{
    public class UserProgress
    {
        public int ProgressId { get; set; }
        public int UserId { get; set; }
        public int ScoreObtained { get; set; }
        public User User { get; set; }
        public int SetId { get; set; }
        public Set Set { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
