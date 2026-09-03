using DataAccess.Dbcontexts;
using DataAccess.Entities;
using Tests.Factories;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Controllers;

public class UsersControllerApiTest: IClassFixture<TestDbFactory>
{
    private TestDbFactory factory;
    private HttpClient client;

    public UsersControllerApiTest(TestDbFactory _factory)
    {
        factory = _factory;
        client = factory.CreateClient();
    }

    [Fact]
    public async Task TestDeleteUser()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = AppUserFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        Assert.Single(context.Users);

        var response = await client.DeleteAsync($"/api/users/{user.Id}");

        Console.WriteLine("response.StatusCode {0}", response.StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // using var verifyScope = factory.Services.CreateAsyncScope();
        // var verifyDb = verifyScope.ServiceProvider.GetRequiredService<AppDbContext>();
        // var deleted = await verifyDb.Users.FindAsync(user.Id);

        context.ChangeTracker.Clear();
        var deleted = await context.Users.FindAsync(user.Id);
        Console.WriteLine("Users count {0}", await context.Users.CountAsync());

        Assert.Null(deleted);
    }

}
