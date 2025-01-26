using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	public class LikesController(ILikeRepository likeRepository) : BaseAPIController
	{
		[HttpPost("{targetUserID:int}")]
		public async Task<ActionResult> ToggleLike(int targetUserID)
		{
			var sourceUserID = User.GetUserID();

			if (sourceUserID == targetUserID) return BadRequest("You cannot like yourself");

			var existingLike = await likeRepository.GetUserLike(sourceUserID, targetUserID);

			if (existingLike is null)
			{
				var like = new UserLike
				{
					SourceUserID = sourceUserID,
					TargetUserID = targetUserID
				};

				likeRepository.AddLike(like);
			}
			else
			{
				likeRepository.DeleteLike(existingLike);
			}

			if (await likeRepository.SaveChanges()) return Ok();

			return BadRequest("Failed to update like");
		}

		[HttpGet("list")]
		public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIDs()
		{
			return Ok(await likeRepository.GetCurrentUserLikeIDs(User.GetUserID()));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUserLikes([FromQuery] LikeParamsHelper likeParamsHelper)
		{
			likeParamsHelper.UserID = User.GetUserID();
			var users = await likeRepository.GetUserLikes(likeParamsHelper);

Response.AddPaginationHeader(users);
			return Ok(users);
		}
	}
}
