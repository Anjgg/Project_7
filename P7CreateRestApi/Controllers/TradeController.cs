using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/trades")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _service;

        public TradeController(ITradeService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerDocumentation("trade", (int)CrudType.GetAll)]
        public async Task<IActionResult> ListAllTrades()
        {
            var trades = await _service.GetAllAsync();
            if (trades == null)
                return NotFound(); //404
            else
                return Ok(trades); //200
        }

        [HttpGet("{trade_id}")]
        [SwaggerDocumentation("trade", (int)CrudType.GetById)]
        public async Task<IActionResult> GetTrade(int trade_id)
        {
            var trade = await _service.GetByIdAsync(trade_id);
            if (trade == null)
                return NotFound(); //404
            else
                return Ok(trade); //200
        }

        [HttpPost]
        [SwaggerDocumentation("trade", (int)CrudType.Create)]
        public async Task<IActionResult> CreateTrade([FromBody] TradeDto tradeDto)
        {
            var createdId = await _service.CreateAsync(tradeDto);
            return CreatedAtAction(nameof(CreateTrade), new { id = createdId }, tradeDto); //201
        }

        [HttpPut("{trade_id}")]
        [SwaggerDocumentation("trade", (int)CrudType.Update)]
        public async Task<IActionResult> UpdateTrade(int trade_id, [FromBody] TradeDto tradeDto)
        {
            var updated = await _service.UpdateAsync(trade_id, tradeDto);

            if (updated == null)
                return NotFound(); //404
            else
                return NoContent(); //200
        }

        [HttpDelete("{trade_id}")]
        [SwaggerDocumentation("trade", (int)CrudType.Delete)]
        public async Task<IActionResult> DeleteTrade(int trade_id)
        {
            var hasBeenDeleted = await _service.DeleteAsync(trade_id);
            if (hasBeenDeleted == false)
                return NotFound(); //404
            else
                return NoContent(); //204
        }
    }
}