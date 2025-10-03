using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace P7CreateRestApi.Services
{
    public interface IAuthenticationService
    {
        public Task<string?> GetJwtToken(LoginDto loginDto);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly UserRepository _repository;

        public AuthenticationService(IConfiguration configuration, UserRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<string?> GetJwtToken(LoginDto loginDto)
        {
            var user = await _repository.FindUser(loginDto);
            
            if (user is null)
                return null;

            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer")!,
                audience: _configuration.GetValue<string>("Jwt:Audience")!,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
