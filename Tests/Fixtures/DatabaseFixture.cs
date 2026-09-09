using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using DataAccess.Dbcontexts;

namespace Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private SqlConnection _connection = null!;
    private Respawner _respawner = null!;

    public TestWebApplicationFactory Factory { get; }

    public DatabaseFixture()
    {
        Factory = new TestWebApplicationFactory();
    }

    public async Task InitializeAsync()
    {
        using var scope = Factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();

        _connection = new SqlConnection("Server=sql1-test,1433;Database=firstapp;User Id=sa;Password=P@ssw0rd123;Encrypt=True;TrustServerCertificate=True;");

        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions{ DbAdapter = DbAdapter.SqlServer });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connection);
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}