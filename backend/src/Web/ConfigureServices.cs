using Application.Common.Interfaces;
using Application.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Web.Filters;
using Web.Services;

namespace Web
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUIServices(this IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IAttributeTypeServices, AttributeTypeServices>();
            services.AddScoped<IAttributeValueServices, AttributeValueServices>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IEmailService, EmailService>();

            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddAutoMapper(typeof(Startup));

            services
            .AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>());


            //Get the url of the api
            services.AddScoped<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                //var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                var absoluteUri = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.Path}";
                return new UriService(absoluteUri);
            });


            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce Api", Version = "v1" });
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

            return services;
        }
    }
}
