using Application.DTO;
using Application.Requests.UserReauests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserRequest user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = await _userService.Add(user);
            var res = new { Id = userId };
            return CreatedAtAction(nameof(GetById), new { id = userId }, res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Update(user);
            if (!result)
                return NotFound();

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
