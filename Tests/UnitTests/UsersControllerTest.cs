using System.Text.Json;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Interfaces;
using Service.Requests;
using Tests.Factories;
using WebApi.Controllers;
using WebApi.Requests;

namespace Tests.UnitTests;

public class UsersControllerTest: UnitTestBase
{
    public UserFactory UserFactory { get; set; }

    public UsersControllerTest()
    {
        UserFactory = new UserFactory();
    }

    [Fact]
    public async Task Test_controller_can_get_users_list()
    {
        List<AppUser> userList = UserFactory.CreateMany(10);
        Task<List<AppUser>> result = Task.FromResult(userList);
        Mock<IUsersService> mockUserService = new Mock<IUsersService>();
        mockUserService.Setup(x => x.GetUsersList()).ReturnsAsync(userList).Verifiable();

        var controller = new UsersController(mockUserService.Object);
        var actionResult = await controller.GetUsers();
        Assert.IsType<OkObjectResult>(actionResult.Result);
        mockUserService.Verify(x => x.GetUsersList(), Times.Exactly(1));
    }

    [Fact]
    public async Task Test_controller_can_get_user_by_id()
    {
        AppUser user = UserFactory.Create();
        Mock<IUsersService> mockUserService = new Mock<IUsersService>();
        mockUserService.Setup(x => x.GetUserById(user.Id)).ReturnsAsync(user).Verifiable();

        var controller = new UsersController(mockUserService.Object);
        ActionResult<AppUser>? actionResult = await controller.GetUser(user.Id);

        mockUserService.Verify(x => x.GetUserById(user.Id), Times.Exactly(1));
        Assert.IsType<OkObjectResult>(actionResult?.Result);
    }

    [Fact]
    public async Task Test_controller_can_create_user()
    {
        AppUser user = UserFactory.Create();
        CreateUserHttpRequest httpRequest = new CreateUserHttpRequest
        {
            Username = user.Username,
            Email = user.Email,
            Password = user.Password
        };

        Mock<IUsersService> mockUserService = new Mock<IUsersService>();
        mockUserService.Setup(x => x.CreateUserAsync(It.IsAny<CreateUserServiceRequest>()))
            .ReturnsAsync(user)
            .Verifiable();
        
        UsersController controller = new UsersController(mockUserService.Object);
        IActionResult actionResult = await controller.CreateUser(httpRequest);

        mockUserService.Verify(x => x.CreateUserAsync(It.Is<CreateUserServiceRequest>(request => 
             request.Username == user.Username &&
             request.Email == user.Email &&
             request.Password == user.Password)), Times.Exactly(1));

        CreatedAtActionResult createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
        AppUser actualUser = Assert.IsType<AppUser>(createdResult.Value);
        Assert.Equal(user.Username, actualUser.Username);
        Assert.Equal(user.Email, actualUser.Email);
        Assert.Equal(user.Password, actualUser.Password);
    }

    [Fact]
    public async Task Test_controller_can_update_user()
    {
        AppUser user = UserFactory.Create();
        AppUser updatedUser = UserFactory.Create();
        Mock<IUsersService> mockUserService = new Mock<IUsersService>();
        mockUserService.Setup(x => x.UpdateUserAsync(It.Is<UpdateUserServiceRequest>(request => 
            request.Id == updatedUser.Id &&
            request.Username == updatedUser.Username &&
            request.Email == updatedUser.Email &&
            request.Password == updatedUser.Password
            )))
            .ReturnsAsync(user)
            .Verifiable();

        UsersController controller = new UsersController(mockUserService.Object);
        IActionResult actionResult = await controller.UpdateUser(new UpdateUserHttpRequest
        {
            Username = updatedUser.Username,
            Email = updatedUser.Email,
            Password = updatedUser.Password
        }, user.Id);

        mockUserService.Verify(x => x.UpdateUserAsync(It.Is<UpdateUserServiceRequest>(request => 
            request.Id == user.Id &&
            request.Username == updatedUser.Username &&
            request.Email == updatedUser.Email &&
            request.Password == updatedUser.Password
            )), Times.Exactly(1));
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public async Task Test_controller_can_delete_user()
    {
        AppUser user = UserFactory.Create();
        Mock<IUsersService> mockUserService = new Mock<IUsersService>();
        mockUserService.Setup(x => x.DeleteUserAsync(It.Is<DeleteUserServiceRequest>(request => request.Id == user.Id)))
            .Returns(Task.CompletedTask)
            .Verifiable();

        UsersController controller = new UsersController(mockUserService.Object);
        IActionResult actionResult = await controller.DeleteUser(user.Id);

        mockUserService.Verify(x => x.DeleteUserAsync(It.Is<DeleteUserServiceRequest>(request => request.Id == user.Id)), Times.Exactly(1));
        Assert.IsType<NoContentResult>(actionResult);
    }
}
