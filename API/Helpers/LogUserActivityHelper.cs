using System;
using API.Extensions;
using API.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

public class LogUserActivityHelper : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var resultContext = await next();
		// Everything happens after this next is happened after ActionResult being executed.

		if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;
		var userID = resultContext.HttpContext.User.GetUserID();

		var dbContext = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
		var user = await dbContext.GetMemberByIDAsync(userID);
		if (user is null) return;

		user.LastActive = DateTime.UtcNow;
		await dbContext.SaveAllAsync();
	}
}
