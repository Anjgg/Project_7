using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/bids")]
    [Authorize]

    public class BidController : ControllerBase
    {
        private readonly IBidService _service;

        public BidController(IBidService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerDocumentation("bid", (int)CrudType.GetAll)]
        public async Task<IActionResult> ListAllBids()
        {
            var bids = await _service.GetAllAsync();

            if (bids == null)
                return NotFound(); //404
            else
                return Ok(bids); //200
        }

        [HttpGet("{bid_id}")]
        [SwaggerDocumentation("bid", (int)CrudType.GetById)]

        public async Task<IActionResult> GetBid(int bid_id)
        {
            var bid = await _service.GetByIdAsync(bid_id);

            if (bid == null)
                return NotFound(); //404
            else
                return Ok(bid); //200
        }

        [HttpPost]
        [SwaggerDocumentation("bid", (int)CrudType.Create)]
        public async Task<IActionResult> CreateBid([FromBody] BidDto bidDto)
        {
            var createdId = await _service.CreateAsync(bidDto);
            
            return CreatedAtAction(nameof(CreateBid), new { id = createdId }, bidDto); //201
        }

        [HttpPut("{bid_id}")]
        [SwaggerDocumentation("bid", (int)CrudType.Update)]
        public async Task<IActionResult> UpdateBid(int bid_id, [FromBody] BidDto bidDto)
        {
            var updated = await _service.UpdateAsync(bid_id, bidDto);

            if (updated == null)
                return NotFound(); //404
            else
                return NoContent(); //204
        }

        [HttpDelete("{bid_id}")]
        [SwaggerDocumentation("bid", (int)CrudType.Delete)]
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