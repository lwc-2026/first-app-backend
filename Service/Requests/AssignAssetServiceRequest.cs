using System;

namespace Service.Requests;

public class AssignAssetServiceRequest
{
    public string UserId { get; }
    public int AssetId { get; }
    public string Description { get; }

    public AssignAssetServiceRequest(string userId, int assetId, string description)
    {
        UserId = userId;
        AssetId = assetId;
        Description = description;
    }
}
