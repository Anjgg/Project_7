namespace P7CreateRestApi.Dto
{
    public record CreateUserDto(string email, string password, string? role);
    public record UserDto(string id, string email, IList<string> roles);
    public record class UpdateUserDto(string? email, string? password, string? role);
}
