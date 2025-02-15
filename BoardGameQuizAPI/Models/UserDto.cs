using BoardGameQuizAPI.Enums;

namespace BoardGameQuizAPI.Models
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Title Title { get; set; }
        public string? Role { get; set; }
    }
}
