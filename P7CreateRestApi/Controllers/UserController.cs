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
    [Route("api/v{version:apiVersion}/users")]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerDocumentation("user", (int)CrudType.GetAll)]
        public IActionResult ListAllUsers()
        {
            var users = _service.GetAllUsers();
            if (users == null)
                return NotFound(); //404
            else
                return Ok(users); //200
        }

        [HttpGet("{user_id}")]
        [SwaggerDocumentation("user", (int)CrudType.GetById)]

        public async Task<IActionResult> GetUser(string user_id)
        {
            var user = await _service.GetByIdAsync(user_id);

            if (user == null)
                return NotFound(); //404
            else
                return Ok(user); //200
        }

        [HttpPost]
        [SwaggerDocumentation("user", (int)CrudType.Create)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var (hasBeenCreated, errors) = await _service.CreateAsync(userDto);
            if (hasBeenCreated is true)
                return CreatedAtAction(nameof(CreateUser), new { Message = "User has been created" }); //201
            else
                return BadRequest(new { Errors = errors }); //400

        }

        [HttpPut("{user_id}")]
        [SwaggerDocumentation("user", (int)CrudType.Update)]
        public async Task<IActionResult> UpdateUser(string user_id, [FromBody] UpdateUserDto userDto)
        {
            var (hasBeenUdpated, errors) = await _service.UpdateAsync(user_id, userDto);

            if (hasBeenUdpated is true)
                return NoContent(); //204
            else if (errors!.Contains("User not found"))
                return NotFound(); //404
            else
                return BadRequest(new { Errors = errors }); //400

        }

        [HttpDelete("{user_id}")]
        [SwaggerDocumentation("user", (int)CrudType.Delete)]
        public async Task<IActionResult> DeleteUser(string user_id)
        {
            var (hasBeenDeleted, errors) = await _service.DeleteAsync(user_id);
            if (hasBeenDeleted is true)
                return NoContent(); //204
            else if (errors!.Contains("User not found"))
                return NotFound(); //404
            else
                return BadRequest(new { Errors = errors }); //400
        }
    }
}