using DataAccess.Dbcontexts;
using DataAccess.Entities;
using DataAccess.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Data.Common;
using System.Text.Json;

namespace Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private DbConnection? _dbConnection;
    private Respawner? _respawner;
    private readonly TestWebApplicationFactory _webApplicationFactory;
    public IServiceProvider Services => _webApplicationFactory.Services;
    public IServiceScope CreateScope() => Services.CreateScope();
    public HttpClient CreateClient() => _webApplicationFactory.CreateClient();

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

    public async Task<HttpClient> ActingAsAsync(HttpClient client)
    {
        using var scope = CreateScope();
        {
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            {
                AppUser? user = await context.Users.FirstOrDefaultAsync(x => x.Username == "testuser");

                if(user is null)
                {
                    using var hmac = new System.Security.Cryptography.HMACSHA512();
                    {
                        var password = "P@ssw0rd123";
                        user = new AppUser
                        {
                            Username = "testuser",
                            Email = "testuser@example.com",
                            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)),
                            PasswordSalt = hmac.Key
                        };
                        context.Users.Add(user);
                        await context.SaveChangesAsync();
                    }
                }

                if(user is not null)
                {
                    using var _client = CreateClient();
                    {
                        WebApi.Requests.LoginHttpRequest loginRequest = new WebApi.Requests.LoginHttpRequest()
                        {
                            Username = user.Username,
                            Password = "P@ssw0rd123",
                        };
                        var result = await _client.PostAsync($"/api/Auth/Login",
                        new StringContent(JsonSerializer.Serialize(loginRequest), System.Text.Encoding.UTF8, "application/json"));
                        var responseContent = await result.Content.ReadAsStringAsync();
                        var responseJson = JsonSerializer.Deserialize<JsonElement>(responseContent);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", responseJson.GetProperty("token").GetString());
                    }
                }
            }
        }
        return client;
    }
}