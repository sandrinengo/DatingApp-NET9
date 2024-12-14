using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

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

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
// The instructor said that we need the MapController middleware to
// map the controller endpoints that we'll be creating.
app.MapControllers();

app.Run();
