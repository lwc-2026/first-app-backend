namespace DataAccess.Entities;

public class AppUser
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Username { get; set; }
    public string? Email { get; set; }
}
