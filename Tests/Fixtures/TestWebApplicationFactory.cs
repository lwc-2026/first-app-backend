using DataAccess.Dbcontexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Fixtures;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // remove original registration

            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // add testing registration

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("Server=sql1-test,1433;Database=firstapp;User Id=sa;Password=P@ssw0rd123;Encrypt=True;TrustServerCertificate=True;");
            });
        });
    }
}