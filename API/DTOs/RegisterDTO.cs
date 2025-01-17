using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
	[Required]
	public string Username { get; set; } = String.Empty;

	[Required]
	public string? KnownAs { get; set; }

	[Required]
	public string? Gender { get; set; }

	[Required]
	public string? DOB { get; set; }

	[Required]
	public string? City { get; set; }

	[Required]
	public string? Country { get; set; }

	[Required]
	[StringLength(10, MinimumLength = 4)]
	public string Password { get; set; } = String.Empty;
}
