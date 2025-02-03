using BoardGameQuizAPI.Enums;

namespace BoardGameQuizAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public Title Title { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
