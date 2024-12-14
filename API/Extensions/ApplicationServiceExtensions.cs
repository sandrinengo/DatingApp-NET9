using System;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this
    IServiceCollection services,
    IConfiguration config)
    {
        // Instructor said we need the service to add the controllers
        // and register them, so we can use the APIs endpoints (that is the app.MapControllers(); below)
        services.AddControllers();

        services.AddDbContext<DataContext>(option =>
        {
            option.UseSqlite(config.GetConnectionString("AppConnectionString"));
        });

        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
