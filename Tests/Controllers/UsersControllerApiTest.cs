using DataAccess.Dbcontexts;
using Tests.Factories;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Controllers;

public class UsersControllerApiTest: IClassFixture<TestDbFactory>
{
    private TestDbFactory testDbFactory;
    private HttpClient client;

    public UsersControllerApiTest(TestDbFactory factory)
    {
        testDbFactory = factory;
        client = testDbFactory.CreateClient();
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
    }

}
