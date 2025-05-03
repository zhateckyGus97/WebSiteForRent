using Application.Interfaces;
using Application.Requests.DealRequests;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DealController : ControllerBase
    {
        private IDealService _dealService;
        public DealController(IDealService dealService)
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
        public async Task<IActionResult> Add([FromBody] CreateDealRequest deal)
        {
            var dealId = await _dealService.Add(deal);
            return Created($"/deal/{dealId}", new { Id = dealId });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDealRequest deal)
        {
            var result = await _dealService.Update(deal);
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
