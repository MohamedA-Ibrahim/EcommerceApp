using System.Reflection;
using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Filters;
using WebApi.Services;

namespace WebApi;

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
        //Install services from other projects
        services.AddInfrastructure(Configuration);
        services
            .AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation(mvcConfiguration=> mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>());

        //Get the facebook settings and register the DI
        var facebookAuthSettings = new FacebookAuthSettings();
        Configuration.Bind(nameof(FacebookAuthSettings), facebookAuthSettings);
        services.AddSingleton(facebookAuthSettings);
        services.AddSingleton<IFacebookAuthService, FacebookAuthService>();
        services.AddHttpClient();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddHttpContextAccessor();

        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo {Title = "Ecommerce Api", Version = "v1"});
            x.OperationFilter<SwaggerFileOperationFilter>();

            //Add the filters
            x.ExampleFilters();

            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            //Add documentation to swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            x.IncludeXmlComments(xmlPath);
        });
        services.AddSwaggerExamplesFromAssemblyOf<Startup>();

        services.AddRazorPages();
        services.AddControllers();


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
        app.UseHsts();

        var swaggerOptions = new SwaggerSettings();
        Configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerOptions);

        app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

        app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });
    }
}