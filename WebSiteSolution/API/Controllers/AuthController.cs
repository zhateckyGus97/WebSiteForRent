using Application.Interfaces;
using Application.Requests;
using Application.Requests.UserRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [EnableRateLimiting("register")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationUserRequest request)
        {
            await authService.Register(request);
            return Created();
        }

        [EnableRateLimiting("login")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await authService.Login(request);
            return Ok(response);
        }
    }
}
