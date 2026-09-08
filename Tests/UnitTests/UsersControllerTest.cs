using WebApi.Controllers;
using Tests.Factories;
using Moq;
using Service.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Tests.UnitTests;

public class UsersControllerTest: UnitTestBase
{
    public UserFactory UserFactory { get; set; }

    public UsersControllerTest()
    {
        UserFactory = new UserFactory();
    }

    [Fact]
    public async Task Test_controller_get_users_list()
    {
        List<AppUser> userList = UserFactory.CreateMany(10);
        Task<List<AppUser>> result = Task.FromResult(userList);
        Mock<IUsersService> mockUserService = new Mock<IUsersService>();
        mockUserService.Setup(x => x.GetUsersList())
            .ReturnsAsync(userList);

        var controller = new UsersController(mockUserService.Object);
        var actionResult = await controller.GetUsers();
        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Test_controller_get_user_by_id()
    {
        AppUser user = UserFactory.Create();
        Mock<IUsersService> mockUserService = new Mock<IUsersService>();
        mockUserService.Setup(x => x.GetUserById(user.Id))
            .ReturnsAsync(user);

        var controller = new UsersController(mockUserService.Object);
        var actionResult = await controller.GetUser(user.Id);
        Assert.IsType<OkObjectResult>(actionResult.Result);
    }
}
