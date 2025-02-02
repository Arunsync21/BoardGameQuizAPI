using BoardGameQuizAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BoardGameQuizAPI.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public LoginResponseModel GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenValidityMins = _configuration.GetValue<int>("Jwt:ExpiryInMinutes");
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(tokenValidityMins);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.MobileNumber)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: tokenExpiryTimeStamp,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.WriteToken(token);


            return new LoginResponseModel
            {
                AccessToken = accessToken,
                UserName = user.Name,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }

        public User Authenticate(LoginRequestModel login)
        {
            var user = _context.Users.SingleOrDefault(u =>
                u.Name == login.UserName &&
                u.MobileNumber == login.Mobile &&
                u.Email == login.Email);

            if (user == null)
            {
                return null; // User not found
            }

            return user;
        }
    }
}
