using System;
using System.ComponentModel.DataAnnotations;
using BusinessModel.Enums;

namespace WebApi.Requests;

public class CreateAssetHttpRequest
{
    [Required]
    [StringLength(100)]
    public string AssetNo { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string SerialNo { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;
    [Required]
    [EnumDataType(typeof(AssetStatus))]
    public AssetStatus Status { get; set; }
    public string? UserId { get; set; }
}
