using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;

namespace Tests.Factories;

public class TestDbFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection connection;

    public TestDbFactory()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }

    protected override void Dispose(bool disposing)
    {
        connection.Dispose();
        base.Dispose(disposing);
    }
}