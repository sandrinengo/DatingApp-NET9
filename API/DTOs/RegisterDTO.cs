using System;

namespace API.DTOs;

public class RegisterDTO
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
