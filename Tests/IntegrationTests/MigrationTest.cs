using BusinessModel.DTOs;
using DataAccess.Dbcontexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Fixtures;
using BusinessModel.Enums;

namespace Tests.IntegrationTests;

public class MigrationTest : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public MigrationTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Database_Should_Be_Migrated_Successfully()
    {
        using var scope = _fixture.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var canConnect = await context.Database.CanConnectAsync();

        canConnect.Should().BeTrue();

        var migrations = await context.Database.GetAppliedMigrationsAsync();

        migrations.Should().NotBeEmpty();
        migrations.Should().Contain(x => x.Contains("InitialCreate"));
        migrations.Should().Contain(x => x.Contains("AddHealthCheckStoredProcedure"));

        var pending = await context.Database.GetPendingMigrationsAsync();

        pending.Should().BeEmpty();

        var userCount = await context.Users.CountAsync();

        userCount.Should().Be(0);
    }

    [Fact]
    public async Task HealthCheck_StoredProcedure_Should_Return_Healthy()
    {
        using var scope = _fixture.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var results = (await context.Database
            .SqlQuery<HealthCheckDto>($"EXEC dbo.HEALTHCHECK")
            .ToListAsync());

        results.Should().ContainSingle();
        results.Single()
            .HealthCheckStatus
            .Should()
            .Be(HealthCheckStatus.Healthy);
    }
}