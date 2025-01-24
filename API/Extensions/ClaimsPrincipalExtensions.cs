using System;
using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
	public static string GetUserName(this ClaimsPrincipal user)
	{
		var userName = user.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("Cannot get username from token");

		return userName;
	}

	public static int GetUserID(this ClaimsPrincipal user)
	{
		var userID = Convert.ToInt32(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get username from token"));

		return userID;
	}
}
