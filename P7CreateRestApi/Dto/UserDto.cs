using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dto
{
    public record CreateUserDto([Required, EmailAddress] string email, [Required, MinLength(8)] string password, [Required] string? role);
    public record UserDto([Required] string id, [Required, EmailAddress] string email, [Required] IList<string> roles);
    public record class UpdateUserDto(string? email, string? password, string? role);
}
