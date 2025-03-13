using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
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
            this._reviewService = reviewService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewService.GetById(id);
            return Ok(review);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewService.GetAll();
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] ReviewDTO review)
        {
            await _reviewService.Add(review);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] ReviewDTO review)
        {
            var result = await _reviewService.Update(review);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewService.Delete(id);
            return Ok(result);
        }
    }
}
