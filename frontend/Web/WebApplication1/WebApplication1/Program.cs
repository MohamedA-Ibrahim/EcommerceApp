using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Helpers;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

//Define connection for the database
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

//All all definitions in program
builder.Services.AddEndpointDefinitions(typeof(Category));

var app = builder.Build();

//Use the definitions in the 
app.UseEndPointDefinitions();

app.Run();





