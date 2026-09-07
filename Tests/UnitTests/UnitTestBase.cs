using Bogus;

namespace Tests.UnitTests;

public abstract class UnitTestBase()
{
    protected Faker Faker { get; } = new Faker();
}