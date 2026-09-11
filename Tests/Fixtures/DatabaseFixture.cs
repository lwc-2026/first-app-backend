using DataAccess.Dbcontexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Data.Common;

namespace Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private DbConnection? _dbConnection;
    private Respawner? _respawner;
    private readonly TestWebApplicationFactory _webApplicationFactory;
    public IServiceProvider Services => _webApplicationFactory.Services;
    public IServiceScope CreateScope() => Services.CreateScope();

    // constructor
    public DatabaseFixture()
    {
        _webApplicationFactory = new TestWebApplicationFactory();
    }

    public async Task InitializeAsync()
    {
        using var scope = CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        _dbConnection = context.Database.GetDbConnection();

        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions{ 
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore =
            [
                "__EFMigrationsHistory"
            ]
        });
    }

    public async Task DisposeAsync()
    {
        if (_dbConnection != null)
        {
            await _dbConnection.DisposeAsync();
        }

        await _webApplicationFactory.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner!.ResetAsync(_dbConnection!);
    }

}