using System;
using BusinessModel.Enums;
using DataAccess.Entities;

namespace Service.Requests;

public class CreateAssetServiceRequest(string serialNo, string assetNo, string model, AssetStatus status)
 : ServiceRequest<Asset>
{
    // Add properties for the asset that need to be set during creation
    public string SerialNo { get; set; } = serialNo;
    public string AssetNo { get; set; } = assetNo;
    public string Model { get; set; } = model;
    public AssetStatus Status { get; set; } = status;

    public override Asset ToEntity()
    {
        Asset asset = new()
        {
            // Map properties from request to asset
            SerialNo = this.SerialNo,
            AssetNo = this.AssetNo,
            Model = this.Model,
            Status = this.Status
        };
        return asset;
    }
}
