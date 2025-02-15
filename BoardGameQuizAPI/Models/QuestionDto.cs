namespace BoardGameQuizAPI.Models
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public int RoleId { get; set; }
        public int SetId { get; set; }
        public int SectionId { get; set; }
        public string QuestionType { get; set; } // Text, Passage, Image, Video
        public string? QuestionText { get; set; }
        public string? Passage { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int TimeLimit { get; set; } = 30; // Default: 30 seconds
    }
}
