namespace BoardGameQuizAPI.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int SetId { get; set; }
        public Set Set { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public string QuestionType { get; set; }
        public string? QuestionText { get; set; }
        public string? Passage { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int TimeLimit { get; set; } = 30;
    }
}
