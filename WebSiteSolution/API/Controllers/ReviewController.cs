using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewService.GetById(id);
            if (review != null)
                return Ok(review);

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewService.GetAll();
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ReviewDTO review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewId = await _reviewService.Add(review);
            var res = new { Id = reviewId };
            return CreatedAtAction(nameof(GetById), new { id = reviewId }, res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ReviewDTO review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reviewService.Update(review);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewService.Delete(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
