using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace P7CreateRestApi.Config;

public class UserLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserLoggingMiddleware> _logger;

    public UserLoggingMiddleware(RequestDelegate next, ILogger<UserLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? userId = null;
        string? userEmail = null;
        string? userRole = null;

        if (context.User.Identity?.IsAuthenticated == true)
        {
            userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            userEmail = context.User.FindFirstValue(ClaimTypes.Email);
            userRole = context.User.FindFirstValue(ClaimTypes.Role);
        }

        var method = context.Request.Method;
        var path = context.Request.Path;

        _logger.LogInformation("Request {Method} {Path} by UserId={UserId} Email={Email} Role= {Role}",
            method, path, userId ?? "Anonymous", userEmail ?? "N/A", userRole);

        await _next(context);
    }
}

