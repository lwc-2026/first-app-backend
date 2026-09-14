using DataAccess.Dbcontexts;
using DataAccess.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.Implementations;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Tests.Factories;
using Tests.Fixtures;

namespace Tests.IntegrationTests;

[Collection("Integration")]
public class UsersServiceTest
{
    private readonly DatabaseFixture _fixture;
    private readonly UserFactory _userFactory;

    public UsersServiceTest(DatabaseFixture fixture, UserFactory userFactory)
    {
        _fixture = fixture;
        _userFactory = userFactory;
        Console.WriteLine($"UserServiceTest Initialized: {GetHashCode()}");
    }

    [Fact]
    public async Task Can_get_user_lists()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        List<AppUser> appUsers = _userFactory.CreateMany(10);

        foreach (AppUser user in appUsers)
        {
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();

        // refresh context

        context.ChangeTracker.Clear();

        UsersService userService = new UsersService(context);

        var result = await userService.GetUsersList();

        // assertions

        result.Should().BeOfType<List<AppUser>>();
        // result.Should().HaveCount(appUsers.Count);
    }

    [Fact]
    public async Task Can_get_one_user()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        AppUser appUser = _userFactory.Create();

        context.Users.Add(appUser);

        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        UsersService userService = new UsersService(context);

        AppUser? result = await userService.GetUserById(appUser.Id);

        // assertions

        result.Should().BeOfType<AppUser>();
        result.Should().BeEquivalentTo(appUser);
    }

}

