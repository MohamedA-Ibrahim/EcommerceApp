using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        //Create the roles for admina and normal users
        var administratorRole = new IdentityRole("Admin");
        var userRole = new IdentityRole("User");

        //Only add if it doesn't exist in the database
        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            await roleManager.CreateAsync(administratorRole);

        if (roleManager.Roles.All(r => r.Name != userRole.Name)) await roleManager.CreateAsync(userRole);

        var administrator = new ApplicationUser {UserName = "admin@gmail.com", Email = "admin@gmail.com"};
        var user = new ApplicationUser {UserName = "user@gmail.com", Email = "user@gmail.com"};

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Admin@123");
            await userManager.AddToRolesAsync(administrator, new[] {administratorRole.Name});
        }

        if (userManager.Users.All(u => u.UserName != user.UserName))
        {
            await userManager.CreateAsync(user, "User@123");
            await userManager.AddToRolesAsync(user, new[] {userRole.Name});
        }
    }

    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.Items.Any())
        {
            context.Items.Add(new Item
            {
                Name = "Shopping"
            });

            await context.SaveChangesAsync();
        }
    }
}