using System;
using System.Collections.Generic;
using System.Text;
using Tests.Factories;
using Tests.Fixtures;

namespace Tests.Collections
{
    [CollectionDefinition("Integration")]
    public class IntegrationCollection: ICollectionFixture<DatabaseFixture>, IAsyncLifetime, ICollectionFixture<UserFactory>

    {
        protected DatabaseFixture _fixture;
        protected UserFactory _userFactory;

        public IntegrationCollection(DatabaseFixture fixture, UserFactory userFactory)
        {
            _fixture = fixture;
            _userFactory = userFactory;
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
}
