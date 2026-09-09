using Xunit;
using Tests.Fixtures;

namespace Tests.Collections;

[CollectionDefinition("Database")]
public class DatabaseCollection: ICollectionFixture<DatabaseFixture>
{
}