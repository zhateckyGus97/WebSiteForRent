using Application.DTO;
using Application.Interfaces;
using Application.Services;
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
            this._dealService = dealService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var deal = await _dealService.GetById(id);
            return Ok(deal);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deals = await _dealService.GetAll();
            return Ok(deals);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery]DealDTO deal)
        {
            await _dealService.Add(deal);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] DealDTO deal)
        {
            var result = await _dealService.Update(deal);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dealService.Delete(id);
            return Ok(result);
        }
    }
}
