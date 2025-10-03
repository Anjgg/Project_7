using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    [Authorize(Roles = "Admin")]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerDocumentation("user", (int)CrudType.GetAll)]
        public async Task<IActionResult> ListAllUsers()
        {
            var users = await _service.GetAllAsync();
            if (users == null)
                return NotFound(); //404
            else
                return Ok(users); //200
        }

        [HttpGet("{user_id}")]
        [SwaggerDocumentation("user", (int)CrudType.GetById)]

        public async Task<IActionResult> GetUser(int user_id)
        {
            var user = await _service.GetByIdAsync(user_id);

            if (user == null)
                return NotFound(); //404
            else
                return Ok(user); //200
        }

        [HttpPost]
        [SwaggerDocumentation("user", (int)CrudType.Create)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var created = await _service.CreateAsync(userDto);

            return CreatedAtAction(nameof(CreateUser), new { id = created.Id }, created); //201
        }

        [HttpPut("{user_id}")]
        [SwaggerDocumentation("user", (int)CrudType.Update)]
        public async Task<IActionResult> UpdateUser(int user_id, [FromBody] UserDto userDto)
        {
            var updated = await _service.UpdateAsync(user_id, userDto);

            if (updated == null)
                return NotFound(); //404
            else
                return NoContent(); //204
        }

        [HttpDelete("{user_id}")]
        [SwaggerDocumentation("user", (int)CrudType.Delete)]
        public async Task<IActionResult> DeleteUser(int user_id)
        {
            var hasBeenDeleted = await _service.DeleteAsync(user_id);
            if (hasBeenDeleted == false)
                return NotFound(); //404
            else
                return NoContent(); //204
        }
    }
}