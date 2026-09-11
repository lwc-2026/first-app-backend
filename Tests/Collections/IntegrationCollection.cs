using System;
using System.Collections.Generic;
using System.Text;
using Tests.Fixtures;

namespace Tests.Collections
{
    [CollectionDefinition("Integration")]
    public class IntegrationCollection: ICollectionFixture<DatabaseFixture>, IAsyncLifetime
    {
        protected DatabaseFixture _fixture = new DatabaseFixture();
        public async Task InitializeAsync()
        {
            await _fixture.ResetDatabaseAsync();
        }

        public async Task DisposeAsync()
        {
            await _fixture.DisposeAsync();
        }
    }
}
