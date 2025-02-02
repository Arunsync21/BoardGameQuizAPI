using BoardGameQuizAPI.Models;
using BoardGameQuizAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameQuizAPI.Controllers
{

    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestModel login)
        {
            var user = _authService.Authenticate(login);

            if (user == null)
            {
                return Unauthorized();
            }

            var loginResponse = _authService.GenerateToken(user);

            return Ok(loginResponse);
        }
    }
}
