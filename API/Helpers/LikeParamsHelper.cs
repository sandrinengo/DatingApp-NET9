using System;

namespace API.Helpers;

public class LikeParamsHelper : PaginationParamsHelper
{
	public int UserID { get; set; }
	public required string Predicate { get; set; } = "liked";
}
