using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/rules")]
    public class RuleController : ControllerBase
    {
        private readonly IRuleService _service;

        public RuleController(IRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerDocumentation("rule", (int)CrudType.GetAll)]
        public async Task<IActionResult> ListAllRules()
        {
            var rules = await _service.GetAllAsync();
            if (rules == null)
                return NotFound(); //404
            else
                return Ok(rules); //200
        }

        [HttpGet("{rule_id}")]
        [SwaggerDocumentation("rule", (int)CrudType.GetById)]
        public async Task<IActionResult> GetRule(int rule_id)
        {
            var rule = await _service.GetByIdAsync(rule_id);
            if (rule == null)
                return NotFound(); //404
            else
                return Ok(rule); //200
        }

        [HttpPost]
        [SwaggerDocumentation("rule", (int)CrudType.Create)]
        public async Task<IActionResult> CreateRule([FromBody] RuleDto ruleDto)
        {
            var createdId = await _service.CreateAsync(ruleDto);
            return CreatedAtAction(nameof(CreateRule), new { id = createdId }, ruleDto); //201
        }

        [HttpPut("{rule_id}")]
        [SwaggerDocumentation("rule", (int)CrudType.Update)]
        public async Task<IActionResult> UpdateRule(int rule_id, [FromBody] RuleDto ruleDto)
        {
            var updated = await _service.UpdateAsync(rule_id, ruleDto);

            if (updated == null)
                return NotFound(); //404
            else
                return Ok(updated); //200
        }

        [HttpDelete("{rule_id}")]
        [SwaggerDocumentation("rule", (int)CrudType.Delete)]
        public async Task<IActionResult> DeleteRule(int rule_id)
        {
            var deleted = await _service.DeleteAsync(rule_id);
            if (deleted)
                return NoContent(); //204
            else
                return NotFound(); //404
        }
    }
}