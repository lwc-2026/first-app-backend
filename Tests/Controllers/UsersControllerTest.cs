using DataAccess.Dbcontexts;
using Microsoft.EntityFrameworkCore;
using WebApi.Controllers;
using DataAccess.Entities;
using Tests.Factories;

namespace Tests.Controllers;

public class UsersControllerTest
{
    private AppDbContext context;
    private UsersController usersController;

    public UsersControllerTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        context = new AppDbContext(options);
        context.Database.EnsureCreated();
        usersController = new UsersController(context);
    }

    [Fact]
    public void TestGetUsers()
    {
        
    }

    [Fact]
    public void TestGetUser()
    {
        
    }

    [Fact]
    public void TestCreateUser()
    {
        
    }

    [Fact]
    public void TestUpdateUser()
    {
        Assert.True(true);
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
