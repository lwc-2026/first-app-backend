using DataAccess.Dbcontexts;
using Microsoft.EntityFrameworkCore;
using WebApi.Controllers;
using DataAccess.Entities;
using Tests.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace Tests.Controllers;

public class UsersControllerApiTest: IClassFixture<WebApplicationFactory<Program>>
{
    private AppDbContext context;
    private UsersController usersController;
    private HttpClient client;

    public UsersControllerApiTest(WebApplicationFactory<Program> factory)
    {
        // var connection = new SqliteConnection("Data Source=:memory:");
        // connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        context = new AppDbContext(options);
        usersController = new UsersController(context);
        client = factory.CreateClient();
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

        // var response = await client.DeleteAsync($"/api/users/{user.Id}");

        // await usersController.DeleteUser(user.Id);

        // context.Users.Remove(user);
        // context.SaveChanges();
        // Console.WriteLine(response.StatusCode);
        // Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        // Assert.Empty(context.Users);
    }

}
