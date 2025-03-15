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
        private ApartmentService _apartmentService;

        public ApartmentController(ApartmentService apartService)
        {
            _apartmentService = apartService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apart = await _apartmentService.GetById(id);
            return Ok(apart);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var aparts = await _apartmentService.GetAll();
            return Ok(aparts);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ApartmentDTO apart)
        {
            await _apartmentService.Add(apart);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ApartmentDTO apart)
        {
            var result = await _apartmentService.Update(apart);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _apartmentService.Delete(id);
            return Ok(result);
        }
    }
}
