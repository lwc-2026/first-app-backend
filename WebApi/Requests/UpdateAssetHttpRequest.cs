using System;
using System.ComponentModel.DataAnnotations;
using BusinessModel.Enums;

namespace WebApi.Requests;

public class UpdateAssetHttpRequest
{
    [StringLength(100)]
    public string AssetNo { get; set; } = string.Empty;
    [StringLength(100)]
    public string SerialNo { get; set; } = string.Empty;
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;
    [EnumDataType(typeof(AssetStatus))]
    public AssetStatus Status { get; set; }
    public string? UserId { get; set; }
}
