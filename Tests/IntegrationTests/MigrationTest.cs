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
    public async Task TestDatabaseMigration()
    {
        using var scope = _fixture.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var canConnect = await context.Database.CanConnectAsync();

        canConnect.Should().BeTrue();

        Console.WriteLine(context.Database.GetConnectionString());

        var migrations = await context.Database.GetAppliedMigrationsAsync();

        migrations.Should().NotBeEmpty();
        migrations.Should().Contain("20260907030308_InitialCreate");
        migrations.Should().Contain("20260907030557_AddHealthCheckStoredProcedure");

        var pending = await context.Database.GetPendingMigrationsAsync();

        pending.Should().BeEmpty();

        var userCount = await context.Users.CountAsync();

        userCount.Should().Be(0);
    }

    [Fact]
    public async Task Test_HealthCheck_Stored_Procedure_can_be_executed()
    {
        using var scope = _fixture.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var result = (await context.Database.SqlQuery<HealthCheckDto>($"EXEC dbo.HEALTHCHECK;")
                .ToListAsync())
                .AsEnumerable().FirstOrDefault();

        result.Should().NotBeNull();

        result!.HealthCheckStatus.Should().Be(HealthCheckStatus.Healthy);
    }
}