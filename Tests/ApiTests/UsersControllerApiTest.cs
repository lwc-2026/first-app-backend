using Azure;
using DataAccess.Dbcontexts;
using DataAccess.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Tests.Factories;
using Tests.Fixtures;

namespace Tests.ApiTests;

[Collection("Integration")]
public class UsersControllerApiTest(DatabaseFixture fixture)
{
    private readonly DatabaseFixture _fixture = fixture;
    private readonly HttpClient _client = fixture.CreateClient();

    [Fact]
    public async Task Test_can_get_users_list()
    {
        using var scope = _fixture.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        UserFactory userFactory = new UserFactory();

        AppUser appuser = userFactory.Create();

        context.Users.Add(appuser);
        await context.SaveChangesAsync();

        var response = await _client.GetAsync("/api/users");

        response.EnsureSuccessStatusCode();

        // more assertions
        var users = await response.Content.ReadFromJsonAsync<List<AppUser>>() ?? new List<AppUser>();
        users.Should().ContainSingle(u => u.Id == appuser.Id);
        users.Should().ContainSingle(u => u.Email == appuser.Email);
        users.Should().ContainSingle(u => u.Username == appuser.Username);
    }
}
