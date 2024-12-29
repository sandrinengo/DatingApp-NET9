using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(24, MinimumLength = 14)]
    public string Password { get; set; } = string.Empty;
}
