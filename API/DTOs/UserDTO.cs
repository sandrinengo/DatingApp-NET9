using System;
using Newtonsoft.Json;

namespace API.DTOs;

public class UserDTO
{
	public required string Username { get; set; }
	public required string KnownAs { get; set; }
	public required string Token { get; set; }
	public required string Gender { get; set; }
	public string? PhotoURL { get; set; }
}
