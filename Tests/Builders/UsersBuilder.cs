using DataAccess.Entities;

namespace Tests.Builders;

public class UsersBuilder
{
    public static AppUser Build(string id, string username, string email, string password)
    {
        return new AppUser
        {
            Id = id,
            Username = username,
            Email = email,
            Password = password
        };
    }
}
