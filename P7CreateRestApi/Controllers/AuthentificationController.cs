using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Services;

namespace P7CreateRestApi.Controllers
{

    [ApiController]
    [AllowAnonymous]    
    [Route("api/v{version:apiVersion}")]
    public class AuthentificationController : ControllerBase
    {
        private readonly IAuthenticationService _service;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthentificationController(IAuthenticationService service, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _service = service;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var user = await _userManager.FindByEmailAsync(req.email);
            if (user is null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, req.password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiry) = _service.GenerateJwtToken(user, roles.ToList());

            return Ok(new AuthResponse(token, expiry));
        }


    }
}
