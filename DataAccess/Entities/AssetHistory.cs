using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessModel.Enums;
using DataAccess.Entities;

namespace DataAccess.Entities;

public class AssetHistory : BaseEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int AssetId { get; set; }
    [ForeignKey(nameof(AssetId))]
    public Asset? Asset { get; set; }
    [Required]
    public AssetHistoryAction Action { get; set; }
    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }
    [Required]
    public required string CreatedByUserId { get; set; }
    [ForeignKey(nameof(CreatedByUserId))]
    public AppUser? CreatedByUser { get; set; }
}
