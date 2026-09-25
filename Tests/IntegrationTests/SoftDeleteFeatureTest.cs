using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Tests.Fixtures;
using Tests.Factories;
using DataAccess.Dbcontexts;
using Xunit;

namespace Tests.IntegrationTests;

[Collection("Integration")]
public class SoftDeleteFeatureTest(DatabaseFixture fixture)
{
    private readonly DatabaseFixture _fixture = fixture;
    private readonly UserFactory _userFactory = new UserFactory();

    [Fact]
    public async Task Delete_User_Should_SoftDelete_Record()
    {
        // Arrange
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        // Assert
        var deletedUser = await context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(deletedUser);
        Assert.True(deletedUser.IsDeleted);
        Assert.NotNull(deletedUser.DeletedAt);
    }

    [Fact]
    public async Task Deleted_User_Should_Not_Be_Returned()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        var users = await context.Users.ToListAsync();
        Assert.DoesNotContain(users, u => u.Id == user.Id);
    }

    [Fact]
    public async Task IgnoreQueryFilters_Should_Return_Deleted_User()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        var deletedUser = await context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(deletedUser);
        Assert.True(deletedUser.IsDeleted);
        Assert.NotNull(deletedUser.DeletedAt);
    }

    [Fact]
    public async Task Restore_User_Should_Make_User_Visible_Again()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        var deletedUser = await context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(deletedUser);

        deletedUser.IsDeleted = false;
        deletedUser.DeletedAt = null;
        await context.SaveChangesAsync();

        var restoredUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(restoredUser);
        Assert.False(restoredUser.IsDeleted);
        Assert.Null(restoredUser.DeletedAt);
    }

    [Fact]
    public async Task Delete_User_Should_Not_Remove_Record_From_Database()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = _userFactory.Create();

        context.Users.Add(user);
        await context.SaveChangesAsync();

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        var count = await context.Users
            .IgnoreQueryFilters()
            .CountAsync(x => x.Id == user.Id);

        Assert.Equal(1, count);
    }
}
