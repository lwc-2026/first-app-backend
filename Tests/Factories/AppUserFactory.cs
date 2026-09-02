namespace Tests.Factories;

using Bogus;
using DataAccess.Entities;

public static class AppUserFactory
{
    private static readonly Faker faker = new();
    public static AppUser Create(
        string? id = null,
        string? username = null,
        string? email = null)
    {
        return new AppUser
        {
            Id = id ?? Guid.NewGuid().ToString(),
            Username = username ?? faker.Internet.UserName(),
            Email = email ?? faker.Internet.Email()
        };
    }
}