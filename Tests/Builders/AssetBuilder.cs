using System;
using Bogus;
using DataAccess.Entities;
using BusinessModel.Enums;

namespace Tests.Builders;

public class AssetBuilder(TimeProvider timeProvider)
{
    private static Faker _faker = new();
    private Asset _asset = new()
    {
        AssetNo = _faker.Random.AlphaNumeric(10),
        SerialNo = _faker.Random.AlphaNumeric(10),
        Model = _faker.Commerce.ProductName(),
        Status = _faker.PickRandom<AssetStatus>(),
        UserId = null,
        CreatedAt = timeProvider.GetUtcNow().UtcDateTime,
        UpdatedAt = timeProvider.GetUtcNow().UtcDateTime,
        DeletedAt = null,
    };

    public Asset Build()
    {
        return _asset;
    }

    public AssetBuilder WithSerialNumber(string serialNumber)
    {
        _asset.SerialNo = serialNumber;
        return this;
    }

    public AssetBuilder WithAssetNumber(string assetNumber)
    {
        _asset.AssetNo = assetNumber;
        return this;
    }

    public AssetBuilder WithModel(string model)
    {
        _asset.Model = model;
        return this;
    }

    public AssetBuilder WithStatus(AssetStatus status)
    {
        _asset.Status = status;
        return this;
    }

    public AssetBuilder WithUserId(string? userId)
    {
        _asset.UserId = userId;
        return this;
    }
    public AssetBuilder WithDeletedAt(DateTime? deletedAt)
    {
        _asset.DeletedAt = deletedAt;
        return this;
    }

    public AssetBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _asset.UpdatedAt = updatedAt;
        return this;
    }

    public AssetBuilder WithCreatedAt(DateTime createdAt)
    {
        _asset.CreatedAt = createdAt;
        return this;
    }
}
