using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessModel.Enums;

namespace DataAccess.Entities;

public class AuditLog : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public AuditAction Action { get; set; } = AuditAction.None;

    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUser? User { get; set; }
}
