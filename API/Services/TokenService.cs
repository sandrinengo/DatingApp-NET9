using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Interfaces;
using API.Models;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
	public string CreateToken(User user)
	{
		var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access Token Key from AppSettings");

		if (tokenKey.Length < 64)
			throw new Exception("Your Token Key should be longer");

		// Remember to add the NuGet System.IdentityModel.Tokens.JWT
		// dotnet add package System.IdentityModel.Tokens.Jwt --version 8.3.0
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

		var claims = new List<Claim>(){
			new(ClaimTypes.NameIdentifier, user.ID.ToString()),
			new(ClaimTypes.Name, user.UserName)
		};

		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // We use HmacSha512Signature that's why we need to check legth > 64
		var tokenDescriptor = new SecurityTokenDescriptor()
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddDays(7),
			SigningCredentials = credentials
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}
