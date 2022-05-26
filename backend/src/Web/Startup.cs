using Application.Common.Interfaces;
using Application.Interfaces;
using Application.Settings;
using Azure.Storage.Blobs;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure(Configuration);
        services.AddWebUIServices();

        //Get the facebook settings and register the DI
        var facebookAuthSettings = new FacebookAuthSettings();
        Configuration.Bind(nameof(FacebookAuthSettings), facebookAuthSettings);
        services.AddSingleton(facebookAuthSettings);
        services.AddSingleton<IFacebookAuthService, FacebookAuthService>();

        var blobStorageSettings = new BlobStorageSettings();
        Configuration.Bind(nameof(blobStorageSettings), blobStorageSettings);
        services.AddSingleton(x => new BlobServiceClient(blobStorageSettings.ConnectionString));
        services.AddScoped<IFileStorageService, BlobStorageService>();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        var swaggerOptions = new SwaggerSettings();
        Configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerOptions);

        app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

        app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
        app.UseDefaultFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });
    }
}