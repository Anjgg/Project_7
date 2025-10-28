using Microsoft.AspNetCore.Identity;

namespace P7CreateRestApi.Config;

public static class RolesSeeder
{
    public static async Task EnsureSeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "DefaultUser" };
        foreach (var r in roles)
        {
            if (!await roleManager.RoleExistsAsync(r))
                await roleManager.CreateAsync(new IdentityRole(r));
        }
    }
}