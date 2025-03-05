using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserInfoController : ControllerBase
    {
        private IUserInfoService _userService;
        public UserInfoController(IUserInfoService userService)
        {
            this._userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        /*[HttpPost]
        public async Task<IActionResult> Add([FromBody] UserInfoDTO user)
        {
            await _userService.Add(user);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserInfoDTO user)
        {
            var result = await _userService.Update(user);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Update([FromQuery] int id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }*/
    }
}
