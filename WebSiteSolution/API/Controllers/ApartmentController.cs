using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApartmentController : ControllerBase
    {
        private ApartmentService _apartmentService;

        public ApartmentController(ApartmentService apartmentService)
        {
            _apartmentService = apartmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apartment = await _apartmentService.GetById(id);
            if (apartment != null)
                return Ok(apartment);

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apartments = await _apartmentService.GetAll();
            return Ok(apartments);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ApartmentDTO apartment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var apartmentId = await _apartmentService.Add(apartment);
            var res = new { Id = apartmentId };
            return CreatedAtAction(nameof(GetById), new { id = apartmentId }, res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ApartmentDTO apartment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _apartmentService.Update(apartment);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _apartmentService.Delete(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
