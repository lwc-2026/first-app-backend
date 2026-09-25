using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Requests;

public class AssignAssetHttpRequest()
{
    [Required]
    public required string UserId { get; set; }
    [Required]
    public required int AssetId { get; set; }
    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }
}
