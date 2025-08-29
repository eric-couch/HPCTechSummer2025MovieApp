using Microsoft.AspNetCore.Identity;

namespace HPCTechSummer2025MovieApp.Services;

public class RoleInitializerService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleInitializerService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task InitializeRolesAsync()
    {
        List<string> roleNames = new List<string>()
        {
            "Admin",
            "User"
        };

        foreach (string roleName in roleNames)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
