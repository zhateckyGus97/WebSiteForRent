using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApartmentController : ControllerBase
    {
        private ApartmentService _apartService;

        public ApartmentController(ApartmentService apartService)
        {
            this._apartService = apartService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apart = await _apartService.GetById(id);
            return Ok(apart);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var aparts = await _apartService.GetAll();
            return Ok(aparts);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] ApartmentDTO apart)
        {
            await _apartService.Add(apart);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] ApartmentDTO apart)
        {
            var result = await _apartService.Update(apart);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _apartService.Delete(id);
            return Ok(result);
        }
    }
}
