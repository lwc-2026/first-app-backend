using DataAccess.Dbcontexts;
using Microsoft.EntityFrameworkCore;
using WebApi.Controllers;
using DataAccess.Entities;
using Tests.Factories;

namespace Tests.Controllers;

public class UsersControllerTest : IClassFixture<TestDbFactory>
{
    private TestDbFactory factory;
    private AppDbContext context;
    private UsersController usersController;

    public UsersControllerTest(TestDbFactory testDbFactory)
    {
        factory = testDbFactory;
        context = factory.context;
        usersController = new UsersController(context);
    }

    [Fact]
    public async Task TestDeleteUser()
    {
        var user = AppUserFactory.Create();
        context.Users.Add(user);
        context.SaveChanges();
        Assert.NotEmpty(context.Users);
        await usersController.DeleteUser(user.Id);
        Assert.Empty(context.Users);
    }

}
