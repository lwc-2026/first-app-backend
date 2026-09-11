using Tests.Fixtures;

namespace Tests.IntegrationTests;

public abstract class IntegrationTestBase(DatabaseFixture fixture): IAsyncLifetime
{
    protected readonly DatabaseFixture _fixture = fixture;

    public async Task InitializeAsync()
    {
        await _fixture.ResetDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        
    }
}