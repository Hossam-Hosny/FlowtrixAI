using FlowtrixAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FlowtrixAI.Infrastructure.Seeders;

public static class UserSeeder
{
    public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
    {
        var adminUserName = "newAdmin";
        var adminEmail = "admin@flowtrix.ai";
        var adminUser = await userManager.FindByNameAsync(adminUserName);

        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                Name = "محمد أدمن",
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin&&2026");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
