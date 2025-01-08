using System;
using API.DTOs;
using API.Extensions;
using API.Models;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfileHelper : Profile
{
	public AutoMapperProfileHelper()
	{
		// ForMember to map the fields which are not the same names between the 2 classes
		CreateMap<User, MemberDTO>()
		.ForMember(d => d.Age, o => o.MapFrom(s => s.DOB.CalculateAge()))
		.ForMember(d => d.PhotoURL, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.URL));//d = destination o = option s = source. Use Null Forgiving ! to remove the NULL warning.
		CreateMap<Photo, PhotoDTO>();
		CreateMap<MemberUpdateDTO, User>();
	}
}
