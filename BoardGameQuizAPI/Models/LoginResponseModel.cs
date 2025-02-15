namespace BoardGameQuizAPI.Models
{
    public class LoginResponseModel
    {
        public User User { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
