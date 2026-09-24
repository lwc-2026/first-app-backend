using System;
using BusinessModel.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

public class Asset : BaseEntity
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
    public string? AssignedUserId { get; set; }

    [ForeignKey(nameof(AssignedUserId))]
    public AppUser? AssignedUser { get; set; }
}
