using DataAccess.Entities;
using Tests.Builders;

namespace Tests.Factories;

public class UserFactory : Factory
{
    public AppUser Create(string? id = null,
                          string? username = null,
                          string? email = null,
                          string? password = null)
    {
        return UsersBuilder.Build(
            id: id ?? base.faker.Random.Guid().ToString(),
            username: username ?? base.faker.Internet.UserName(),
            email: email ?? base.faker.Internet.Email(),
            password: password ?? base.faker.Internet.Password()
        );
    }
    
    public List<AppUser> CreateMany(int count)
    {
        List<AppUser> userList = new List<AppUser>();
        for (int i = 0; i < count; i++)
        {
            var user = Create();
            userList.Add(user);
        }
        return userList;
    }
}