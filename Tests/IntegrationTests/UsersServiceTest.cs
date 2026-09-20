using Bogus;
using DataAccess.Dbcontexts;
using DataAccess.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.Implementations;
using Service.Requests;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using Tests.Factories;
using Tests.Fixtures;

namespace Tests.IntegrationTests;

[Collection("Integration")]
public class UsersServiceTest
{
    private readonly DatabaseFixture _fixture;
    private readonly UserFactory _userFactory;
    private readonly Faker _faker;

    public UsersServiceTest(DatabaseFixture fixture, UserFactory userFactory)
    {
        _fixture = fixture;
        _userFactory = userFactory;
        _faker = new Faker();
    }

    internal UsersService SetupUserService(AppDbContext context) => new UsersService(context);

    [Fact]
    public async Task Can_get_user_lists()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userService = this.SetupUserService(context);
        List<AppUser> appUsers = _userFactory.CreateMany(10);

        foreach (AppUser user in appUsers)
        {
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();

        // refresh context

        context.ChangeTracker.Clear();

        var result = await userService.GetUsersList();

        // assertions
        result.Should().BeOfType<List<AppUser>>();
    }

    [Fact]
    public async Task Can_get_one_user()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userService = this.SetupUserService(context);
        AppUser appUser = _userFactory.Create();

        context.Users.Add(appUser);

        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        AppUser? result = await userService.GetUserById(appUser.Id);

        // assertions

        result.Should().BeOfType<AppUser>();
        result.Should().BeEquivalentTo(appUser);
    }

    [Fact]
    public async Task Can_create_one_user()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userService = this.SetupUserService(context);

        string username = _faker.Internet.UserName();
        string email = _faker.Internet.Email();
        string password = _faker.Internet.Password();

        CreateUserServiceRequest serviceRequest = new CreateUserServiceRequest(
            username: username,
            email: email,
            password: password
        );

        AppUser? result = await userService.CreateUserAsync(serviceRequest);

        context.ChangeTracker.Clear();

        var query = context.Users.Where(x => x.Username == username && x.Email == email);

        List<AppUser> queryResult = await query.ToListAsync();

        queryResult.Should().HaveCount(1);
        queryResult.Single().Username.Should().Be(username);
        queryResult.Single().Email.Should().Be(email);
    }


    [Fact]
    public async Task Can_Delete_one_user()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userService = this.SetupUserService(context);

        AppUser user = _userFactory.Create();

        context.Users.Add(user);
        await context.SaveChangesAsync();

        DeleteUserServiceRequest serviceRequest = new DeleteUserServiceRequest(user.Id);

        context.ChangeTracker.Clear();

        var query = context.Users.Where(x => x.Id == user.Id);

        List<AppUser> queryResult = await query.ToListAsync();

        queryResult.Should().HaveCount(1);
        queryResult.Single().Id.Should().Be(user.Id);
        queryResult.Single().Username.Should().Be(user.Username);
        queryResult.Single().Email.Should().Be(user.Email);

        await userService.DeleteUserAsync(serviceRequest);

        context.ChangeTracker.Clear();

        queryResult = await query.ToListAsync();

        queryResult.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Can_Update_one_user()
    {
        using var scope = _fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userService = this.SetupUserService(context);

        AppUser user = _userFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var query = context.Users.Where(x => x.Id == user.Id);

        context.ChangeTracker.Clear();

        List<AppUser> queryResult = await query.ToListAsync();

        queryResult.Should().HaveCount(1);
        queryResult.Single().Id.Should().Be(user.Id);
        queryResult.Single().Username.Should().Be(user.Username);
        queryResult.Single().Email.Should().Be(user.Email);

        string updatedUsername = _faker.Internet.UserName();
        string updatedEmail = _faker.Internet.Email();
        string updatedPassword = _faker.Internet.Password();

        UpdateUserServiceRequest serviceRequest = new UpdateUserServiceRequest() { 
            Id = user.Id,
            Username = updatedUsername,
            Email = updatedEmail,
            Password = updatedPassword
        };

        var response = await userService.UpdateUserAsync(serviceRequest);

        context.ChangeTracker.Clear();
        queryResult = await query.ToListAsync();

        queryResult.Should().HaveCount(1);
        queryResult.Single().Id.Should().Be(user.Id);
        queryResult.Single().Username.Should().Be(updatedUsername);
        queryResult.Single().Email.Should().Be(updatedEmail);
    }
}

