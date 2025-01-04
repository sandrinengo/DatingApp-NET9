using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
// Middleware section

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
// The instructor said that we need the MapController middleware to
// map the controller endpoints that we'll be creating.
app.MapControllers();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
	var context = services.GetRequiredService<DataContext>();

	await context.Database.MigrateAsync();
	await Seed.SeedUsers(context);
}
catch (Exception ex)
{
	var logger = services.GetRequiredService<ILogger<Program>>();

	logger.LogError(ex, "An error occurred during migration");
}

app.Run();
