using System;
using BusinessModel.Enums;
using BusinessModel.DTOs;

namespace BusinessModel.DTOs;

public class AssetDto
{
    public int Id { get; set; }
    public required string SerialNo { get; set; }
    public required string AssetNo { get; set; }
    public required string Model { get; set; }
    public required AssetStatus Status { get; set; }
    public UserDto? User { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<AssetHistoryDto>? AssetHistories { get; set; } = new List<AssetHistoryDto>();
}
