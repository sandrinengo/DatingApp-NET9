using System;
using API.Data;
using API.DTOs;
using API.Helpers;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class LikeRepository(DataContext context, IMapper mapper) : ILikeRepository
{
	public void AddLike(UserLike userLike)
	{
		context.Likes.Add(userLike);
	}

	public void DeleteLike(UserLike userLike)
	{
		context.Likes.Remove(userLike);
	}

	public async Task<IEnumerable<int>> GetCurrentUserLikeIDs(int currentUserID)
	{
		return await context.Likes
		.Where(x => x.SourceUserID == currentUserID)
		.Select(x => x.TargetUserID)
		.ToListAsync();
	}

	public async Task<UserLike?> GetUserLike(int sourceuserID, int targetUserID)
	{
		return await context.Likes.FindAsync(sourceuserID, targetUserID);
	}

	public async Task<PagedListHelper<MemberDTO>> GetUserLikes(LikeParamsHelper likeParamsHelper)
	{
		var likes = context.Likes.AsQueryable();
		IQueryable<MemberDTO> query;
		query = likeParamsHelper.Predicate switch
		{
			"liked" => likes.Where(x => x.SourceUserID == likeParamsHelper.UserID).Select(x => x.TargetUser).ProjectTo<MemberDTO>(mapper.ConfigurationProvider),
			"likedBy" => likes.Where(x => x.TargetUserID == likeParamsHelper.UserID).Select(x => x.SourceUser).ProjectTo<MemberDTO>(mapper.ConfigurationProvider),
			_ => await GetDefaultQuery(likeParamsHelper.UserID, likes)
		};
		// Old way
		// switch (likeParamsHelper.Predicate)
		// {
		// 	case "liked": return await likes.Where(x => x.SourceUserID == likeParamsHelper.UserID).Select(x => x.TargetUser).ProjectTo<MemberDTO>(mapper.ConfigurationProvider).ToListAsync();
		// 	case "likedBy": return await likes.Where(x => x.TargetUserID == likeParamsHelper.UserID).Select(x => x.SourceUser).ProjectTo<MemberDTO>(mapper.ConfigurationProvider).ToListAsync();
		// 	default:
		// 		var likeIDs = await GetCurrentUserLikeIDs(likeParamsHelper.UserID);
		// 		return await likes.Where(x => x.TargetUserID == likeParamsHelper.UserID && likeIDs.Contains(x.SourceUserID)).Select(x => x.SourceUser).ProjectTo<MemberDTO>(mapper.ConfigurationProvider).ToListAsync();
		// }

		return await PagedListHelper<MemberDTO>.CreateAsync(query, likeParamsHelper.PageNumber, likeParamsHelper.PageSize);
	}

	private async Task<IQueryable<MemberDTO>> GetDefaultQuery(int userId, IQueryable<UserLike> likes)
	{
		var likeIDs = await GetCurrentUserLikeIDs(userId);
		return likes.Where(x => x.TargetUserID == userId && likeIDs.Contains(x.SourceUserID))
					.Select(x => x.SourceUser)
					.ProjectTo<MemberDTO>(mapper.ConfigurationProvider);
	}

	public async Task<bool> SaveChanges()
	{
		return await context.SaveChangesAsync() > 0;
	}
}

public interface ILikeRepository
{
	Task<UserLike?> GetUserLike(int sourceuserID, int targetUserID);
	Task<PagedListHelper<MemberDTO>> GetUserLikes(LikeParamsHelper likeParamsHelper);
	Task<IEnumerable<int>> GetCurrentUserLikeIDs(int currentUserID);
	void DeleteLike(UserLike userLike);
	void AddLike(UserLike userLike);
	Task<bool> SaveChanges();
}