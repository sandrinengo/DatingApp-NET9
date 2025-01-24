using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Models;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseAPIController
	{
		#region Properties
		#endregion

		#region Constructors
		#endregion

		#region ActionResults
		[HttpGet] // api/user
		public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers([FromQuery] UserParamsHelper userParams)
		{
			userParams.CurrentUsername = User.GetUserName();
			var userList = await userRepository.GetMembersAsync(userParams);
			Response.AddPaginationHeader(userList);

			return Ok(userList);
		}

		[HttpGet("{id:int}")] // api/user/3
		public async Task<ActionResult<MemberDTO>> GetUser(int id)
		{
			var user = await userRepository.GetMemberByIDAsync(id);

			return user == null ? NotFound() : user;
		}

		[HttpGet("{username}")] // api/user/3
		public async Task<ActionResult<MemberDTO>> GetUser(string username)
		{
			var user = await userRepository.GetMemberByUserNameAsync(username);

			return user == null ? NotFound() : user;
		}

		[HttpPut]
		public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
		{
			var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());

			if (user is null) return BadRequest("No username found in token");

			mapper.Map(memberUpdateDTO, user);

			if (await userRepository.SaveAllAsync()) return NoContent();

			return BadRequest("Failed to update the user");
		}

		[HttpPost("add-photo")]
		public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
		{
			var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
			if (user is null) return BadRequest("Cannot update user");

			var result = await photoService.AddPhotoAsync(file);
			if (result.Error is not null) return BadRequest("WHY");//result.Error.Message

			var photo = new Photo
			{
				URL = result.SecureUrl.AbsoluteUri,
				PublicID = result.PublicId
			};
			if (user.Photos.Count == 0) photo.IsMain = true;

			user.Photos.Add(photo);
			if (await userRepository.SaveAllAsync())
				return CreatedAtAction(nameof(GetUser),
				new { userName = user.UserName }, mapper.Map<PhotoDTO>(photo));

			return BadRequest("Problem adding photo");
		}

		[HttpPut("set-main-photo/{photoID:int}")]
		public async Task<ActionResult> SetMainPhoto(int photoID)
		{
			var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
			if (user is null) return BadRequest("Cannot find user");

			var photo = user.Photos.FirstOrDefault(x => x.ID == photoID);

			if (photo is null || photo.IsMain) return BadRequest("Cannot use this as a main photo");

			var currentMainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);

			if (currentMainPhoto is not null) currentMainPhoto.IsMain = false;
			photo.IsMain = true;

			if (await userRepository.SaveAllAsync()) return NoContent();

			return BadRequest("Problem setting main photo");
		}

		[HttpDelete("delete-photo/{photoID:int}")]
		public async Task<ActionResult> DeletePhoto(int photoID)
		{
			var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
			if (user is null) return BadRequest("Cannot find user");

			var photo = user.Photos.FirstOrDefault(x => x.ID == photoID);

			if (photo is null || photo.IsMain) return BadRequest("This photo cannot be deleted");
			if (photo.PublicID is not null)
			{
				var result = await photoService.DeletePhotoAsync(photo.PublicID);
				if (result.Error is not null) return BadRequest(result.Error.Message);
			}
			user.Photos.Remove(photo);
			if (await userRepository.SaveAllAsync()) return Ok();

			return BadRequest("Problem deleting photo");
		}
		#endregion

		#region Methods
		#endregion
	}
}
