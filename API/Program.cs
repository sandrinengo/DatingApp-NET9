using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Instructor said we need the service to add the controllers
// and register them, so we can use the APIs endpoints (that is the app.MapControllers(); below)
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("AppConnectionString"));
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Middleware section
/*
// The instructor wants to comment out
// the pipelines below. He said we do not need it
// for this tutorial.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
*/

// The instructor said that we need the MapController middleware to
// map the controller endpoints that we'll be creating.
app.MapControllers();

app.Run();
