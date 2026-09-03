using DataAccess.Dbcontexts;
using WebApi.Controllers;
using Tests.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.UnitTests;

public class UsersControllerTest: TestBase, IClassFixture<TestDbFactory>
{
    private TestDbFactory factory;

    public UsersControllerTest(TestDbFactory testDbFactory)
    {
        factory = testDbFactory;
    }

    [Fact]
    public async Task TestDeleteUser()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        UsersController usersController = new UsersController(context);
        var user = AppUserFactory.Create();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        Assert.NotEmpty(context.Users);
        await usersController.DeleteUser(user.Id);
        Assert.Empty(context.Users);
        await this.ResetDatabase(context);
    }

}
