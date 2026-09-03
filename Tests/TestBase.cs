using DataAccess.Dbcontexts;

namespace Tests;

public class TestBase
{
    public async Task ResetDatabase(AppDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}