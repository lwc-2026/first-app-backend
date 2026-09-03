using DataAccess.Dbcontexts;
using Tests.Factories;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace Tests.ApiTests;

public class UsersControllerApiTest: TestBase, IClassFixture<TestDbFactory>
{
    private TestDbFactory testDbFactory;
    private HttpClient client;

    public UsersControllerApiTest(TestDbFactory factory)
    {
        testDbFactory = factory;
        client = testDbFactory.CreateClient();
    }

    [Fact]
    public async Task TestGetUsers()
    {
        using var scope = testDbFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = AppUserFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var response = await client.GetAsync($"/api/users");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseData = await response.Content.ReadAsStringAsync();
        Assert.Contains(user.Id.ToString(), responseData);
        if(!string.IsNullOrEmpty(user.Email))
            Assert.Contains(user.Email, responseData);
        Assert.Contains(user.Username, responseData);
        await this.ResetDatabase(context);
    }

    [Fact]
    public async Task TestGetUserById()
    {
        using var scope = testDbFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = AppUserFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var response = await client.GetAsync($"/api/users/{user.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseData = await response.Content.ReadAsStringAsync();
        Assert.Contains(user.Id.ToString(), responseData);
        if(!string.IsNullOrEmpty(user.Email))
            Assert.Contains(user.Email, responseData);
        Assert.Contains(user.Username, responseData);
        await this.ResetDatabase(context);
    }

    [Fact]
    public async Task TestCreateUser()
    {
        using var scope = testDbFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = AppUserFactory.Create();

        var response = await client.PostAsJsonAsync($"/api/users", user);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        context.ChangeTracker.Clear();
        var created = await context.Users.FindAsync(user.Id);
        Assert.NotNull(created);
        Assert.Equal(user.Email, created.Email);
        Assert.Equal(user.Username, created.Username);
        await this.ResetDatabase(context);
    }

    [Fact]
    public async Task TestUpdateUser()
    {
        using var scope = testDbFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = AppUserFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var updatedUser = AppUserFactory.Create();
        updatedUser.Id = user.Id;

        var response = await client.PutAsJsonAsync($"/api/users/{user.Id}", updatedUser);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context.ChangeTracker.Clear();
        var updated = await context.Users.FindAsync(user.Id);
        Assert.NotNull(updated);
        Assert.Equal(updatedUser.Email, updated.Email);
        Assert.Equal(updatedUser.Username, updated.Username);
        await this.ResetDatabase(context);
    }

    [Fact]
    public async Task TestDeleteUser()
    {
        using var scope = testDbFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = AppUserFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        Assert.Single(context.Users);

        var response = await client.DeleteAsync($"/api/users/{user.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        context.ChangeTracker.Clear();
        var deleted = await context.Users.FindAsync(user.Id);
        Assert.Null(deleted);
        await this.ResetDatabase(context);
    }

}
