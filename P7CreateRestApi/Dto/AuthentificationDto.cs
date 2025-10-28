namespace P7CreateRestApi.Dto
{
    public record LoginRequest(string email, string password);
    public record AuthResponse(string token, DateTime expiration);
}
