using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;
using P7CreateRestApi.SwaggerConfig;

namespace P7CreateRestApi.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Username and password must be provided.");
            }

            var token = _service.GetJwtToken(loginDto);

            if (token is null)
            {
                return Unauthorized();
            }
            return Ok(new { token });
        }
    }
}
