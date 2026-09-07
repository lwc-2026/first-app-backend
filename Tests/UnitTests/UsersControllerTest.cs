using WebApi.Controllers;
using Tests.Factories;
using Service.Implementations;
using Moq;
using DataAccess.Entities;

namespace Tests.UnitTests;

public class UsersControllerTest: UnitTestBase
{
    public UserFactory UserFactory { get; set; }

    public UsersControllerTest()
    {
        UserFactory = new UserFactory();
    }

    [Fact]
    public void Test_controller_get_users_list()
    {
        List<AppUser> userList = UserFactory.CreateMany(10);
        Task<List<AppUser>> result = Task.FromResult(userList);
        var mockUserService = new Mock<UsersService>();
        mockUserService.Setup(i => i.GetUsersList())
            .Returns(result);

        var controller = new UsersController(mockUserService.Object);
        var actionResult = controller.GetUsers();
    }
}
