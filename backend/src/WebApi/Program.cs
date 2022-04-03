using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using WebApi;

namespace WebUI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<ApplicationDbContext>();
                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
                    await ApplicationDbContextSeed.SeedSampleDataAsync(context);
             
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>());
    }
}