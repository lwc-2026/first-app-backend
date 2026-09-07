using Tests.Factories;

namespace Tests.IntegrationTests;

public abstract class IntegrationTestBase(TestDbFactory factory) : IClassFixture<TestDbFactory>
{
    protected readonly HttpClient Client = factory.CreateClient();
}