using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities;

public class AppUser
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Username { get; set; }
    public string? Email { get; set; }
}
