using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models;
using P7CreateRestApi.Services;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class BidController : ControllerBase
    {
        private readonly IBidService _service;

        public BidController(IBidService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("bids")]
        public async Task<IActionResult> ListAllBids()
        {
            var bids = await _service.GetAllAsync();

            return Ok(bids); //200
        }

        [HttpGet]
        [Route("bid/{bid_id}")]
        public async Task<IActionResult> GetBid(int bid_id)
        {
            var bid = await _service.GetByIdAsync(bid_id);

            if (bid == null)
                return NotFound(); //404
            else
                return Ok(bid); //200
        }

        [HttpPost]
        [Route("bid")]
        public async Task<IActionResult> CreateBid([FromBody] BidDto bidDto)
        {
            var created = await _service.CreateAsync(bidDto);

            return CreatedAtAction(nameof(CreateBid), new { id = created.Id }, created); //201
        }

        [HttpPost]
        [Route("bid/{bid_id}")]
        public async Task<IActionResult> UpdateBid(int bid_id, [FromBody] BidDto bidDto)
        {
            var updated = await _service.UpdateAsync(bid_id, bidDto);

            if (updated == null)
                return NotFound(); //404
            else
                return NoContent(); //204
        }

        [HttpDelete]
        [Route("bid/{bid_id}")]
        public async Task<IActionResult> DeleteBid(int bid_id)
        {
            var hasBeenDeleted = await _service.DeleteAsync(bid_id);
            if (hasBeenDeleted == false)
                return NotFound(); //404
            else
                return NoContent(); //204
        }
    }
}