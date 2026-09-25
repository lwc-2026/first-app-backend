using DataAccess.Entities;
using Tests.Builders;

namespace Tests.Factories;

public class UserFactory : Factory
{
    public AppUser Create(string? id = null, string? username = null, string? email = null, string? password = null, 
                            DateTime? createdAt = null, DateTime? updatedAt = null, bool? isDeleted = null, DateTime? deletedAt = null)
    {
        password ??= base.faker.Internet.Password();
        var hmac = new System.Security.Cryptography.HMACSHA512();
        var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        var passwordSalt = hmac.Key;

        return UsersBuilder.Build(
            id: id ?? base.faker.Random.Guid().ToString(),
            username: username ?? base.faker.Internet.UserName(),
            email: email ?? base.faker.Internet.Email(),
            passwordHash: passwordHash,
            passwordSalt: passwordSalt,
            createdAt: createdAt ?? DateTime.UtcNow,
            updatedAt: updatedAt ?? DateTime.UtcNow,
            isDeleted: isDeleted ?? false,
            deletedAt: deletedAt
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