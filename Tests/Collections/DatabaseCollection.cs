using Tests.Fixtures;

namespace Tests.Collections;

[CollectionDefinition("Database")]
public class DatabaseCollection: ICollectionFixture<DatabaseFixture>, IAsyncLifetime
{
    protected DatabaseFixture _fixture;

    public DatabaseCollection(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await _fixture.ResetDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }
}