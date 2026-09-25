using System;

namespace Service.Requests;

public class ReturnAssetServiceRequest
{
    public string UserId { get; }
    public int AssetId { get; }
    public string Description { get; }

    public ReturnAssetServiceRequest(string userId, int assetId, string description)
    {
        UserId = userId;
        AssetId = assetId;
        Description = description;
    }
}
