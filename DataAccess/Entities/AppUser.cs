using System.ComponentModel.DataAnnotations;
using BusinessModel.DTOs;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Entities;

public class AppUser : BaseEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public required string Username { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public required byte[] PasswordHash { get; set; }
    [Required]
    public required byte[] PasswordSalt { get; set; }
    public UserDto ToDto()
    {
        return new UserDto
        {
            Id = this.Id,
            Username = this.Username,
            Email = this.Email
        };
    }
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    public ICollection<Asset> Assets { get; set; } = new List<Asset>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
