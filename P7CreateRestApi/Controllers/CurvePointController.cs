using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/curve-points")]
    public class CurvePointController : ControllerBase
    {
        private readonly ICurvePointService _service;

        public CurvePointController(ICurvePointService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerDocumentation("curve point", (int)CrudType.GetAll)]
        public async Task<IActionResult> ListAllCurvePoint()
        {
            var curvePoint = await _service.GetAllAsync();
            if (curvePoint == null)
                return NotFound(); //404
            else
                return Ok(curvePoint); //200
        }

        [HttpGet("{curve_point_id}")]
        [SwaggerDocumentation("curve point", (int)CrudType.GetById)]
        public async Task<IActionResult> GetCurvePoint(int curve_point_id)
        {
            var curvePoint = await _service.GetByIdAsync(curve_point_id);

            if (curvePoint == null)
                return NotFound(); //404
            else
                return Ok(curvePoint); //200
        }

        [HttpPost]
        [SwaggerDocumentation("curve point", (int)CrudType.Create)]
        public async Task<IActionResult> CreateCurvePoint([FromBody]CurvePointDto curvePoint)
        {
            var created = await _service.CreateAsync(curvePoint);

            return CreatedAtAction(nameof(CreateCurvePoint), new { id = created.Id }, created); //201
        }

        [HttpPut("{curve_point_id}")]
        [SwaggerDocumentation("curve point", (int)CrudType.Update)]
        public async Task<IActionResult> UpdateCurvePoint(int curve_point_id, [FromBody] CurvePointDto curvePointDto)
        {
            var updated = await _service.UpdateAsync(curve_point_id, curvePointDto);

            if (updated == null)
                return NotFound(); //404
            else
                return NoContent(); //204
        }

        [HttpDelete("{curve_point_id}")]
        [SwaggerDocumentation("curve point", (int)CrudType.Delete)]
        public async Task<IActionResult> DeleteCurvePoint(int curve_point_id)
        {
            var hasBeenDeleted = await _service.DeleteAsync(curve_point_id);
            if (hasBeenDeleted == false)
                return NotFound(); //404
            else
                return NoContent(); //204
        }
    }
}