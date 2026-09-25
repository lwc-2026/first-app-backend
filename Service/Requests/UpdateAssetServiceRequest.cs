using System;
using BusinessModel.Enums;
using DataAccess.Entities;

namespace Service.Requests;

public class UpdateAssetServiceRequest(int id, string? serialNo, string? assetNo, string? model, AssetStatus? status)
{
    public int Id { get; set; } = id;
    public string? SerialNo { get; set; } = serialNo;
    public string? AssetNo { get; set; } = assetNo;
    public string? Model { get; set; } = model;
    public AssetStatus? Status { get; set; } = status;
}
