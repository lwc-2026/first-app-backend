using System;
using System.Threading.Tasks;
using BusinessModel.Enums;
using DataAccess.Entities;
using DataAccess.Dbcontexts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using Service.Requests;
using Tests.Factories;
using Tests.Fixtures;
using Xunit;

namespace Tests.IntegrationTests;

[Collection("Integration")]
public class AssetServiceTest(DatabaseFixture fixture)
{
    private readonly DatabaseFixture _fixture = fixture;
    private readonly AssetFactory _assetFactory = new(fixture.TimeProvider);

    [Fact]
    public async Task Can_GetAssets()
    {
        var asset = _assetFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>()!;
        IAssetService _assetService = scope.ServiceProvider.GetService<IAssetService>()!;
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        Assert.IsType<Asset>(asset);
        IEnumerable<Asset> assets = await _assetService!.GetAssetsAsync();

        assets.Should().NotBeNull();
        assets.Should().Contain(a => a.Id == asset.Id);
    }

    [Fact]
    public async Task Can_GetAssetById()
    {
        Asset asset = _assetFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>()!;
        IAssetService _assetService = scope.ServiceProvider.GetService<IAssetService>()!;
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        Assert.IsType<Asset>(asset);
        Asset fetchedAsset = await _assetService!.GetAssetByIdAsync(asset.Id);

        fetchedAsset.Should().NotBeNull();
        fetchedAsset.Id.Should().Be(asset.Id);
    }

    [Fact]
    public async Task Can_CreateAsset()
    {
        Asset asset = _assetFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        IAssetService _assetService = scope.ServiceProvider.GetService<IAssetService>()!;
        var createRequest = new CreateAssetServiceRequest(asset.SerialNo, asset.AssetNo, asset.Model, asset.Status);
        Asset createdAsset = await _assetService!.CreateAssetAsync(createRequest);

        AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>()!;
        context.ChangeTracker.Clear();
        var fetchedAsset = await context.Assets.FindAsync(createdAsset.Id);

        fetchedAsset.Should().NotBeNull();
        fetchedAsset.Id.Should().Be(createdAsset.Id);
    }

    [Fact]
    public async Task Can_UpdateAsset()
    {
        Asset asset = _assetFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>()!;
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var updateRequest = new UpdateAssetServiceRequest(asset.Id, "UpdatedSerialNo", "UpdatedAssetNo", "UpdatedModel", AssetStatus.InUse);
        IAssetService _assetService = scope.ServiceProvider.GetService<IAssetService>()!;
        await _assetService!.UpdateAssetAsync(updateRequest);

        context.ChangeTracker.Clear();
        var updatedAsset = await context.Assets.FindAsync(asset.Id);

        updatedAsset.Should().NotBeNull();
        updatedAsset.SerialNo.Should().Be("UpdatedSerialNo");
        updatedAsset.AssetNo.Should().Be("UpdatedAssetNo");
        updatedAsset.Model.Should().Be("UpdatedModel");
        updatedAsset.Status.Should().Be(AssetStatus.InUse);
        updatedAsset.Id.Should().Be(asset.Id);
    }

    [Fact]
    public async Task Can_DeleteAsset()
    {
        // Placeholder for integration test logic
        Asset asset = _assetFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>()!;
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        IAssetService _assetService = scope.ServiceProvider.GetService<IAssetService>()!;
        await _assetService!.DeleteAssetAsync(asset.Id);

        context.ChangeTracker.Clear();
        var deletedAsset = await context.Assets.FindAsync(asset.Id);
        deletedAsset.Should().BeNull();
    }
}
