using System;
using DataAccess.Dbcontexts;
using DataAccess.Entities;
using System.Linq;
using System.Threading.Tasks;
namespace DataAccess.Seeder;

public class TestUserSeeder
{
    public static async Task Seed(AppDbContext dbContext)
    {
        // Add test users here
        if(dbContext.Users.Any() == false)
        {
            var hmac = new System.Security.Cryptography.HMACSHA512();
            dbContext.Users.Add(new AppUser
            {
                Username = "ericsong",
                Email = "ericsong@gmail.com",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("P@ssw0rd123")),
                PasswordSalt = hmac.Key
            });
            await dbContext.SaveChangesAsync();
        }
    }
}
