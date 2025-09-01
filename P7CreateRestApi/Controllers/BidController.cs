using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    
    public class BidController : ControllerBase
    {
        private readonly IBidService _service;

        public BidController(IBidService service)
        {
            _service = service;
        }

        [HttpGet("bids")]
        [SwaggerDocumentation("List all bids", "Create a list of all bids present in the database", (int)CrudType.GetAll)]
        public async Task<IActionResult> ListAllBids()
        {
            var bids = await _service.GetAllAsync();
            if (bids == null)
                return NotFound(); //404
            else
                return Ok(bids); //200
        }

        [HttpGet("bid/{bid_id}")]
        [SwaggerDocumentation("Get one bid", "Retrieve a specific bid in the database", (int)CrudType.GetById)]

        public async Task<IActionResult> GetBid(int bid_id)
        {
            var bid = await _service.GetByIdAsync(bid_id);

            if (bid == null)
                return NotFound(); //404
            else
                return Ok(bid); //200
        }

        [HttpPost("bid")]
        [SwaggerDocumentation("Add one bid", "Create a new bid in the database", (int)CrudType.Create)]
        public async Task<IActionResult> CreateBid([FromBody] BidDto bidDto)
        {
            var created = await _service.CreateAsync(bidDto);

            return CreatedAtAction(nameof(CreateBid), new { id = created.Id }, created); //201
        }

        [HttpPost("bid/{bid_id}")]
        [SwaggerDocumentation("Update one bid", "Update an existing bid store in database", (int)CrudType.Update)]
        public async Task<IActionResult> UpdateBid(int bid_id, [FromBody] BidDto bidDto)
        {
            var updated = await _service.UpdateAsync(bid_id, bidDto);

            if (updated == null)
                return NotFound(); //404
            else
                return NoContent(); //204
        }

        [HttpDelete("bid/{bid_id}")]
        [SwaggerDocumentation("Delete one bid", "Delete a specific bid in the database", (int)CrudType.Delete)]
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