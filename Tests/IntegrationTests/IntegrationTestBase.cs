using Tests.Fixtures;

namespace Tests.IntegrationTests;

[Collection("Database")]
public abstract class IntegrationTestBase: IClassFixture<TestWebApplicationFactory>, IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    protected readonly DatabaseFixture _fixture;
    protected readonly HttpClient _client;

    protected IntegrationTestBase(TestWebApplicationFactory factory, DatabaseFixture fixture)
    {
        _fixture = fixture;
        _client = factory.CreateClient();
    }

    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
}