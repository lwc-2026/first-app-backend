using System;

namespace Service.Requests;

public class AssignAssetServiceRequest
{
    public int Id { get; }
    public string UserId { get; }
    public int AssetId { get; }
    public string Description { get; }

    public AssignAssetServiceRequest(int id, string userId, int assetId, string description)
    {
        Id = id;
        UserId = userId;
        AssetId = assetId;
        Description = description;
    }
}
