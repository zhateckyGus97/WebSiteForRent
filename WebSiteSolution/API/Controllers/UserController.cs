using Api.Extensions;
using Application.Requests.UserRequests;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        public UserController(UserService userService)
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

        [HttpGet("UserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.GetUserId();
            if(!userId.HasValue)
            {
                return NotFound();
            }
            var user = await _userService.GetById(userId.Value);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

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
