using Ecommerce.WebUI.Api;
using Ecommerce.WebUI.Models.User;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocalization(o => o.ResourcesPath = "Languages");
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IAuthenticatedUser, AuthenticatedUser>();
builder.Services.AddSingleton<IApiHelper, ApiHelper>();
builder.Services.AddScoped<ICategoryEndpoint, CategoryEndpoint>();
builder.Services.AddScoped<IItemEndpoint, ItemEndpoint>();
builder.Services.AddScoped<IImageEndpoint, ImageEndpoint>();


builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages();
app.UseDefaultFiles();
app.UseRouting();
app.UseAuthentication();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
