using System;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Repositories;
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
		services.AddScoped<IPhotoService, PhotoService>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<ILikeRepository, LikeRepository>();
		services.AddScoped<LogUserActivityHelper>();
		services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		services.Configure<CloudinarySettingsHelper>(config.GetSection("CloudinarySettings"));

		return services;
	}
}
