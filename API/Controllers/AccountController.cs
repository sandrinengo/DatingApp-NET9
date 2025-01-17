using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseAPIController
	{
		#region Private Variables
		#endregion

		#region Properties
		#endregion

		#region Constructors
		#endregion

		#region ActionResults
		[HttpPost("register")]
		public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
		{
			if (await UserExists(registerDTO.Username))
				return BadRequest("Username exists");

			using var hmac = new HMACSHA256();
			var user = mapper.Map<User>(registerDTO);
			user.UserName = registerDTO.Username.ToLower();
			user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
			user.PasswordSalt = hmac.Key;

			context.Users.Add(user);
			await context.SaveChangesAsync();

			return new UserDTO
			{
				Username = user.UserName,
				KnownAs = user.KnownAs,
				Token = tokenService.CreateToken(user)
			};
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
		{
			var user = await context.Users
			.Include(p => p.Photos)
			.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDTO.Username.ToLower());

			if (user is null)
			{
				return Unauthorized("Invalid user");
			}

			//using var hmac = new HMACSHA256(user.PasswordSalt);
			using var hmac = new HMACSHA512(user.PasswordSalt);
			var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

			for (int i = 0; i < computerHash.Length; i++)
			{
				if (computerHash[i] != user.PasswordHash[i])
					return Unauthorized("Invalid password");
			}

			return new UserDTO
			{
				Username = user.UserName,
				KnownAs = user.KnownAs,
				Token = tokenService.CreateToken(user),
				PhotoURL = user.Photos.FirstOrDefault(x => x.IsMain)?.URL
			};
		}
		#endregion

		#region Methods
		private async Task<bool> UserExists(string userName)
		{
			return await context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
		}
		#endregion
	}
}
