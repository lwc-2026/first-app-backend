using System;
using BusinessModel.Enums;
using BusinessModel.DTOs;
using System.Linq.Expressions;

namespace BusinessModel.DTOs;

public class AssetHistoryDto
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public AssetHistoryAction Action { get; set; }
    public required string Description { get; set; }
    public UserDto? AssignedUser { get; set; }
    public UserDto? CreatedByUser { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
