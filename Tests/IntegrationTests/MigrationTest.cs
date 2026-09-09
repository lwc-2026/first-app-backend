using Tests.Fixtures;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace Tests.IntegrationTests;

public class MigrationTest : IntegrationTestBase
{
    public MigrationTest(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task TestDatabaseMigration()
    {
        // Arrange
        var response = await Client.GetAsync("/api/healthcheck");
        Console.WriteLine(JsonSerializer.Serialize(response));
        // Assert
        response.EnsureSuccessStatusCode();
    }
}