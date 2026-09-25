using DataAccess.Dbcontexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Fixtures;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            // remove original registration

            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // add testing registration

            services.AddScoped<SoftDeleteInterceptor>();
            services.AddScoped<AuditTimestampInterceptor>();
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseSqlServer("Server=sql1-test,1433;Database=firstapp;User Id=sa;Password=P@ssw0rd123;Encrypt=True;TrustServerCertificate=True;");
                options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
                options.AddInterceptors(sp.GetRequiredService<AuditTimestampInterceptor>());
            });
        });
    }
}