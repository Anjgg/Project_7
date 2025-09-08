using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ratings")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _service;

        public RatingController(IRatingService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerDocumentation("rating", (int)CrudType.GetAll)]
        public async Task<IActionResult> ListAllRating()
        {
            var ratings = await _service.GetAllAsync();
            if (ratings == null)
                return NotFound(); //404
            else
                return Ok(ratings); //200
        }

        [HttpGet("{rating_id}")]
        [SwaggerDocumentation("rating", (int)CrudType.GetById)]
        public async Task<IActionResult> GetRating(int rating_id)
        {
            var rating = await _service.GetByIdAsync(rating_id);

            if (rating == null)
                return NotFound(); //404
            else
                return Ok(rating); //200
        }

        [HttpPost]
        [SwaggerDocumentation("rating", (int)CrudType.Create)]
        public async Task<IActionResult> CreateRating([FromBody] RatingDto ratingDto)
        {
            var created = await _service.CreateAsync(ratingDto);

            return CreatedAtAction(nameof(CreateRating), new { id = created.Id }, created); //201
        }

        [HttpPut("{rating_id}")]
        [SwaggerDocumentation("rating", (int)CrudType.Update)]
        public async Task<IActionResult> UpdateRating(int rating_id, [FromBody] RatingDto ratingDto)
        {
            var updated = await _service.UpdateAsync(rating_id, ratingDto);

            if (updated == null)
                return NotFound(); //404
            else
                return NoContent(); //204
        }

        [HttpDelete("{rating_id}")]
        [SwaggerDocumentation("rating", (int)CrudType.Delete)]
        public async Task<IActionResult> DeleteRating(int rating_id)
        {
            var hasBeenDeleted = await _service.DeleteAsync(rating_id);
            if (hasBeenDeleted == false)
                return NotFound(); //404
            else
                return NoContent(); //204
        }
    }
}