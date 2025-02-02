namespace BoardGameQuizAPI.Models
{
    public class UserResponseDto
    {
        public int ResponseId { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public int ResponseTime { get; set; } // Time taken to answer (in seconds)
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    }
}
