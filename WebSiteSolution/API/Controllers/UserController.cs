using Api.Extensions;
using Application.Interfaces;
using Application.Requests.UserRequests;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user != null)
                return Ok(user);

            return NotFound();
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetByEmail(email);
            if (user != null)
                return Ok(user);

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RegistrationUserRequest user)
        {
            var userId = await _userService.Add(user);
            return Created($"/user/{userId}", new {Id = userId});
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest user)
        {
            var result = await _userService.Update(user);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
