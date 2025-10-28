using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Config;
using P7CreateRestApi.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P7CreateRestApi.Services
{
    public interface IAuthenticationService
    {
        public (string token, DateTime expiration) GenerateJwtToken(User user, List<string> roles);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtOptions _opts;

        public AuthenticationService(IOptions<JwtOptions> opts)
        {
            _opts = opts.Value;
        }

        public (string token, DateTime expiration) GenerateJwtToken(User user, List<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            ;

            var expires = DateTime.UtcNow.AddMinutes(_opts.ExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _opts.Issuer,
                audience: _opts.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, expires);
        }
    }
}
