using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DealController : ControllerBase
    {
        private DealService _dealService;
        public DealController(DealService dealService)
        {
            _dealService = dealService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var deal = await _dealService.GetById(id);
            if (deal != null)
                return Ok(deal);

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deals = await _dealService.GetAll();
            return Ok(deals);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]DealDTO deal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dealId = await _dealService.Add(deal);
            var res = new { Id = dealId };
            return CreatedAtAction(nameof(GetById), new { id = dealId }, res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DealDTO deal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dealService.Update(deal);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dealService.Delete(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
