namespace BoardGameQuizAPI.Models
{
    public class Option
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
