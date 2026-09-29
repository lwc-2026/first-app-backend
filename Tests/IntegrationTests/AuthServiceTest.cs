using System.Security.Cryptography;
using System.Text;
using BusinessModel.DTOs;
using DataAccess.Dbcontexts;
using DataAccess.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using Tests.Fixtures;
using Xunit;

namespace Tests.IntegrationTests;

[Collection("Integration")]
public class AuthServiceTest(DatabaseFixture fixture)
{
    private readonly IAuthService _authService = fixture.Services.GetRequiredService<IAuthService>();
    private readonly AppDbContext _context = fixture.Services.GetRequiredService<AppDbContext>();

    [Fact]
    public async Task Can_Login_With_Valid_Credentials()
    {
        var user = await CreateUserAsync("alice", "P@ssw0rd123");

        var result = await _authService.AuthenticateAsync("alice", "P@ssw0rd123");

        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
        result.RefreshTokenExpiresAt.Should().BeAfter(DateTime.UtcNow);

        var savedToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.UserId == user.Id);

        savedToken.Should().NotBeNull();
        savedToken!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Cannot_Login_With_Invalid_Password()
    {
        await CreateUserAsync("bob", "P@ssw0rd123");

        var result = await _authService.AuthenticateAsync("bob", "WrongPassword!");

        result.Should().BeNull();
    }

    [Fact]
    public async Task Can_Refresh_Valid_Refresh_Token()
    {
        var user = await CreateUserAsync("charlie", "P@ssw0rd123");

        var login = await _authService.AuthenticateAsync("charlie", "P@ssw0rd123");
        login.Should().NotBeNull();

        var refreshed = await _authService.RefreshTokenAsync(login!.RefreshToken);

        refreshed.Should().NotBeNull();
        refreshed!.Token.Should().NotBeNullOrWhiteSpace();
        refreshed.RefreshToken.Should().NotBeNullOrWhiteSpace();
        refreshed.Token.Should().NotBe(login.Token);
        refreshed.RefreshToken.Should().NotBe(login.RefreshToken);
    }

    [Fact]
    public async Task Cannot_Refresh_Expired_Or_Revoked_Token()
    {
        var user = await CreateUserAsync("diana", "P@ssw0rd123");

        var login = await _authService.AuthenticateAsync("diana", "P@ssw0rd123");
        login.Should().NotBeNull();

        var token = await _context.RefreshTokens
            .SingleAsync(rt => rt.UserId == user.Id);

        token.RevokedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var refreshed = await _authService.RefreshTokenAsync(login!.RefreshToken);

        refreshed.Should().BeNull();
    }

    private async Task<AppUser> CreateUserAsync(string username, string password)
    {
        using var hmac = new HMACSHA512();
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var user = new AppUser
        {
            Username = username,
            Email = $"{username}@example.com",
            PasswordHash = hmac.ComputeHash(passwordBytes),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}