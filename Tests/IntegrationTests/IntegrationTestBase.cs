using Tests.Fixtures;

namespace Tests.IntegrationTests;

[Collection("Database")]
public abstract class IntegrationTestBase: IAsyncLifetime
{
    protected readonly DatabaseFixture Fixture;
    protected HttpClient Client => Fixture.Factory.CreateClient();

    protected IntegrationTestBase(DatabaseFixture fixture)
    {
        this.Fixture = fixture;
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual Task DisposeAsync() => Fixture.ResetDatabaseAsync();
}