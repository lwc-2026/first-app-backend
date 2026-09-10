using DataAccess.Dbcontexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Fixtures;

namespace Tests.IntegrationTests;

public class MigrationTest : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public MigrationTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task TestDatabaseMigration()
    {
        using var scope = _fixture.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var canConnect = await context.Database.CanConnectAsync();

        canConnect.Should().BeTrue();

        Console.WriteLine(context.Database.GetConnectionString());

        var migrations = await context.Database.GetAppliedMigrationsAsync();

        migrations.Should().NotBeEmpty();

        var pending = await context.Database.GetPendingMigrationsAsync();

        pending.Should().BeEmpty();
    }
}