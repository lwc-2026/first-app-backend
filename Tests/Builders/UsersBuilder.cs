using DataAccess.Entities;

namespace Tests.Builders;

public class UsersBuilder
{
    public static AppUser Build(string id, string username, string email, byte[] passwordHash, byte[] passwordSalt)
    {
        return new AppUser
        {
            Id = id,
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
    }
}
