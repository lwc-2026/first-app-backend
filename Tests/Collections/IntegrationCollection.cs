using System;
using System.Collections.Generic;
using System.Text;
using Tests.Fixtures;

namespace Tests.Collections
{
    [CollectionDefinition("Integration")]
    public class IntegrationCollection: ICollectionFixture<DatabaseFixture>
    {
    }
}
