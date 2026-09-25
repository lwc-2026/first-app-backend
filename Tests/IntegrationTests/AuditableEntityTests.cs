using System;
using DataAccess.Dbcontexts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tests.Factories;
using Tests.Fixtures;

namespace Tests.IntegrationTests;

[Collection("Integration")]
public class AuditableEntityTests(DatabaseFixture databaseFixture)
{
    private readonly DatabaseFixture _databaseFixture = databaseFixture;
    private readonly UserFactory _userFactory = new UserFactory();
    [Fact]
    public async Task CreatedAt_and_UpdatedAt_can_be_set_on_creation()
    {
        using var scope = _databaseFixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // write console line with CreatedAt and UpdatedAt
        Console.WriteLine($"CreatedAt: {user.CreatedAt}, UpdatedAt: {user.UpdatedAt}");

        user.CreatedAt.Should().NotBe(default);
        user.UpdatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task UpdatedAt_changes_on_update_and_CreatedAt_remains_unchanged()
    {
        using var scope = _databaseFixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var originalUpdatedAt = user.UpdatedAt;
        var originalCreatedAt = user.CreatedAt;

        // simulate some update
        user.Username = "Updated Name";
        await context.SaveChangesAsync();
        
        originalCreatedAt.Should().Be(user.CreatedAt);
        if(originalUpdatedAt is DateTime time) user.UpdatedAt.Should().BeAfter(time);

    }

    [Fact]
    public async Task UpdatedAt_changes_when_soft_deleted()
    {
        using var scope = _databaseFixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var originalUpdatedAt = user.UpdatedAt;

        context.Users.Remove(user);

        await context.SaveChangesAsync();

        if(originalUpdatedAt is DateTime time) user.UpdatedAt.Should().BeAfter(time);
    }

    [Fact]
    public async Task UpdatedAt_is_not_changed_when_no_changes_exist()
    {
        using var scope = _databaseFixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var originalUpdatedAt = user.UpdatedAt;

        await context.SaveChangesAsync();

        user.UpdatedAt.Should().Be(originalUpdatedAt);
    }


}
