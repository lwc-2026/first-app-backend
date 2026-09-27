using System;

namespace Service.Requests;

public class ReturnAssetServiceRequest
{
    public int AssetId { get; }
    public string Description { get; }

    public ReturnAssetServiceRequest(int assetId, string description)
    {
        AssetId = assetId;
        Description = description;
    }
}
