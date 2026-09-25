using DataAccess.Entities;

namespace Tests.Builders;

public class UsersBuilder
{
    public static AppUser Build(string id, string username, string email, byte[] passwordHash, byte[] passwordSalt,
                                DateTime createdAt, DateTime updatedAt, bool isDeleted, DateTime? deletedAt)
    {
        return new AppUser
        {
            Id = id,
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            IsDeleted = isDeleted,
            DeletedAt = deletedAt
        };
    }
}
