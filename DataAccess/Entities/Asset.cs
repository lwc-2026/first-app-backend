using System;
using BusinessModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[EntityTypeConfiguration(typeof(AssetConfiguration))]
public class Asset : BaseEntity, ISoftDelete
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string AssetNo { get; set; }
    [Required]
    public required string SerialNo { get; set; }
    [Required]
    public required string Model { get; set; }
    [Required]
    public required AssetStatus Status { get; set; }
    public string? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public AppUser? User { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public ICollection<AssetHistory> AssetHistories { get; set; } = [];
}
