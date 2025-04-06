using Application.Requests.ApartmentRequests;
using Application.Services;
using Domain.Entities;
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
        public async Task<IActionResult> Add([FromBody] CreateApartmentRequest apartment)
        {
            var apartmentId = await _apartmentService.Add(apartment);
            return Created($"/apartment/{apartmentId}", new { Id = apartmentId });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateApartmentRequest apartment)
        {
            var result = await _apartmentService.Update(apartment);
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
