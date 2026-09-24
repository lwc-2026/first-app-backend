using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

public class RefreshToken: BaseEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Token { get; set; }
    [Required]
    public required DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }
    [Required]
    public required string UserId { get; set; }
    [Required]
    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;
}
