using System;
using API.Data;
using API.DTOs;
using API.Helpers;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
	public async Task<MemberDTO?> GetMemberByIDAsync(int id)
	{
		return await context.Users
		.Where(x => x.ID == id)
		 .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
		 .SingleOrDefaultAsync();
	}

	public async Task<PagedListHelper<MemberDTO>> GetMembersAsync(UserParamsHelper userParams)
	{
		var query = context.Users.AsQueryable();
		query = query.Where(x => x.UserName != userParams.CurrentUsername);

		if (userParams.Gender != null)
		{
			query = query.Where(x => x.Gender == userParams.Gender);
		}

		var minDOB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
		var maxDOB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
		query = query.Where(x => x.DOB >= minDOB && x.DOB <= maxDOB);

		query = userParams.OrderBy switch
		{
			"created" => query.OrderByDescending(x => x.CreatedDate),
			_ => query.OrderByDescending(x => x.LastActive)
		};

		return await PagedListHelper<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
	}

	public async Task<MemberDTO?> GetMemberByUserNameAsync(string userName)
	{
		return await context.Users
		.Where(x => x.UserName == userName)
		 .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
		 .SingleOrDefaultAsync();
	}

	public async Task<User?> GetUserByIDAsync(int id)
	{
		return await context.Users.FindAsync(id);
	}

	public async Task<User?> GetUserByUserNameAsync(string userName)
	{
		return await context.Users
		.Include(x => x.Photos)
		.SingleOrDefaultAsync(x => x.UserName == userName);
	}

	public async Task<IEnumerable<User>> GetUsersAsync()
	{
		return await context.Users
		.Include(x => x.Photos)
		.ToListAsync();
	}

	public async Task<bool> SaveAllAsync()
	{
		return await context.SaveChangesAsync() > 0;
	}

	public void Update(User user)
	{
		context.Entry(user).State = EntityState.Modified;
	}
}

public interface IUserRepository
{
	void Update(User user);
	Task<bool> SaveAllAsync();
	Task<IEnumerable<User>> GetUsersAsync();
	Task<User?> GetUserByIDAsync(int id);
	Task<User?> GetUserByUserNameAsync(string userName);
	Task<PagedListHelper<MemberDTO>> GetMembersAsync(UserParamsHelper userParamsHelper);
	Task<MemberDTO?> GetMemberByIDAsync(int id);
	Task<MemberDTO?> GetMemberByUserNameAsync(string userName);
}