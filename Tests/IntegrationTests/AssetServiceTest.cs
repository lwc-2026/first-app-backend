using System;
using System.Threading.Tasks;
using BusinessModel.Enums;
using DataAccess.Entities;
using DataAccess.Dbcontexts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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
    private readonly UserFactory _userFactory = new();

    [Fact]
    public async Task Can_GetAssets()
    {
        var asset = _assetFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>()!;
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>()!;
        var user = _userFactory.Create();
        asset.UserId = user.Id;
        context.Users.Add(user);
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        Assert.IsType<Asset>(asset);
        IEnumerable<Asset> assets = await _assetService!.GetAssetsAsync();

        assets.Should().NotBeNull();
        assets.Should().Contain(a => a.Id == asset.Id);

        var fetchedAsset = assets.Single(a => a.Id == asset.Id);
        fetchedAsset.User.Should().NotBeNull();
        // fetchedAsset.User!.Id.Should().Be(asset.User!.Id);    
    }

    [Fact]
    public async Task Can_GetAssetById()
    {
        Asset asset = _assetFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
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
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var createRequest = new CreateAssetServiceRequest(asset.SerialNo, asset.AssetNo, asset.Model, asset.Status);
        Asset createdAsset = await _assetService!.CreateAssetAsync(createRequest);

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
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var updateRequest = new UpdateAssetServiceRequest(asset.Id, "UpdatedSerialNo", "UpdatedAssetNo", "UpdatedModel", AssetStatus.InUse);
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
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        await _assetService!.DeleteAssetAsync(asset.Id);

        context.ChangeTracker.Clear();
        var deletedAsset = await context.Assets.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Id == asset.Id);
        deletedAsset.Should().NotBeNull();
        deletedAsset.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Can_AssignUserToAsset()
    {
        Asset asset = _assetFactory.Create();
        asset.UserId = null;
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
        var user = _userFactory.Create();
        // asset without userId prepared
        context.Assets.Add(asset);
        context.Users.Add(user);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var assignRequest = new UpdateAssetServiceRequest(asset.Id, null, null, null, null, user.Id);
        await _assetService!.UpdateAssetAsync(assignRequest);

        context.ChangeTracker.Clear();
        var updatedAsset = await context.Assets.FirstOrDefaultAsync(a => a.Id == asset.Id);
        updatedAsset.Should().NotBeNull();
        updatedAsset.UserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task Can_throw_exception_when_assigning_nonexistent_user_to_asset()
    {
        Asset asset = _assetFactory.Create();
        asset.UserId = null;
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var nonExistentUserId = "nonexistent-user-id";
        var assignRequest = new UpdateAssetServiceRequest(asset.Id, null, null, null, null, nonExistentUserId);
        Func<Task> act = async () => await _assetService!.UpdateAssetAsync(assignRequest);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {nonExistentUserId} not found.");
    }

    [Fact]
    public async Task Can_Create_History_When_Submit_Return_Asset()
    {
        Asset asset = _assetFactory.Create();
        AppUser user = _userFactory.Create();
        asset.UserId = user.Id;
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
        context.Users.Add(user);
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var returnRequest = new ReturnAssetServiceRequest(user.Id, asset.Id, "Returning asset");
        await _assetService!.ReturnAssetAsync(returnRequest, user.Id);

        context.ChangeTracker.Clear();
        var returnedAsset = await context.Assets.FirstOrDefaultAsync(a => a.Id == asset.Id);
        returnedAsset.Should().NotBeNull();
        returnedAsset.UserId.Should().BeNull();

        // assert asset history exists
        var assetHistory = await context.AssetHistories.FirstOrDefaultAsync(h => h.AssetId == asset.Id);
        assetHistory.Should().NotBeNull();
        assetHistory.Action.Should().Be(AssetHistoryAction.Returned);
        assetHistory.CreatedByUserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task Can_Create_History_When_Submit_Assign_Asset()
    {
        Asset asset = _assetFactory.Create();
        AppUser user = _userFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
        context.Users.Add(user);
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var assignRequest = new AssignAssetServiceRequest(user.Id, asset.Id, "Assigning asset");
        await _assetService!.AssignAssetAsync(assignRequest, user.Id);

        context.ChangeTracker.Clear();
        var assignedAsset = await context.Assets.FirstOrDefaultAsync(a => a.Id == asset.Id);
        assignedAsset.Should().NotBeNull();
        assignedAsset.UserId.Should().Be(user.Id);

        // assert asset history exists
        var assetHistory = await context.AssetHistories.FirstOrDefaultAsync(h => h.AssetId == asset.Id);
        assetHistory.Should().NotBeNull();
        assetHistory.Action.Should().Be(AssetHistoryAction.Assigned);
        assetHistory.CreatedByUserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task AssignAsset_Should_ForceFailure_When_Requested()
    {
        Asset asset = _assetFactory.Create();
        AppUser user = _userFactory.Create();
        using var scope = _fixture.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IAssetService _assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
        context.Users.Add(user);
        context.Assets.Add(asset);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var assignRequest = new AssignAssetServiceRequest(user.Id, asset.Id, "Assigning asset");
        Func<Task> act = async () => await _assetService!.AssignAssetAsync(assignRequest, user.Id, forceFailure: true);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Forced failure after assigning asset.");

        // check no history generated
        var assetHistory = await context.AssetHistories.FirstOrDefaultAsync(h => h.AssetId == asset.Id);
        assetHistory.Should().BeNull();
    }
}
