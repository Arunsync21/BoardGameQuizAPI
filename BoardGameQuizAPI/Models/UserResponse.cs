namespace BoardGameQuizAPI.Models
{
    public class UserResponse
    {
        public int ResponseId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int OptionId { get; set; }
        public Option Option { get; set; }
        public int ResponseTime { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    }
}
