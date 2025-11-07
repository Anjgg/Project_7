using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dto
{
    public record LoginRequest([Required, EmailAddress] string email, [Required] string password);
    public record AuthResponse(string token, DateTime expiration);
}
