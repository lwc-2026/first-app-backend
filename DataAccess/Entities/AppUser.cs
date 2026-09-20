using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities;

public class AppUser
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
}
