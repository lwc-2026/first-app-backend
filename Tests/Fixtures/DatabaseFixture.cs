using DataAccess.Dbcontexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Data.Common;

namespace Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private DbConnection _connection = null!;
    private Respawner _respawner = null!;
    private readonly TestWebApplicationFactory _factory;
    public IServiceProvider Services => _factory.Services;

    // constructor
    public DatabaseFixture()
    {
        _factory = new TestWebApplicationFactory();
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        _connection = context.Database.GetDbConnection();

        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions{ 
            DbAdapter = DbAdapter.SqlServer,
            //TablesToIgnore = 
            //[
            //    "__EFMigrationsHistory"
            //]
        });
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connection);
    }

}