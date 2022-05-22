using Domain.Entities;
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

        if (roleManager.Roles.All(r => r.Name != userRole.Name))
            await roleManager.CreateAsync(userRole);

        var administrator = new ApplicationUser { UserName = "admin@gmail.com", Email = "admin@gmail.com", PhoneNumber = "01045876541", ProfileName = "The Admin" };
        var user1 = new ApplicationUser { UserName = "user1@gmail.com", Email = "user1@gmail.com", PhoneNumber = "01046547891", ProfileName = "user 1" };
        var user2 = new ApplicationUser { UserName = "user2@gmail.com", Email = "user2@gmail.com", PhoneNumber = "01045678912", ProfileName = "user 2" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Admin@123");
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }

        if (userManager.Users.All(u => u.UserName != user1.UserName))
        {
            await userManager.CreateAsync(user1, "User@123");
            await userManager.AddToRolesAsync(user1, new[] { userRole.Name });
        }

        if (userManager.Users.All(u => u.UserName != user2.UserName))
        {
            await userManager.CreateAsync(user2, "User@123");
            await userManager.AddToRolesAsync(user2, new[] { userRole.Name });
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