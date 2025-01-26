using System;

namespace API.Models;

public class UserLike
{
	public User SourceUser { get; set; } = null!;
	public int SourceUserID { get; set; }
	public User TargetUser { get; set; } = null!;
	public int TargetUserID { get; set; }
}
