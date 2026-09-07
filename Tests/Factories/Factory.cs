using Bogus;

namespace Tests.Factories;

public abstract class Factory
{
    protected readonly Faker faker = new();

}