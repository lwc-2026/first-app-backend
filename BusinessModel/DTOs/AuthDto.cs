using System;

namespace BusinessModel.DTOs;

public class AuthDto
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
}
