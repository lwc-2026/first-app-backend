using System;
using DataAccess.Dbcontexts;
using DataAccess.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Seeder;

public class TestAssetSeeder
{
    public static async Task Seed(AppDbContext dbContext)
    {
        // Add test assets here
        if(dbContext.Assets.Any() == false)
        {
            dbContext.Assets.Add(new Asset
            {
                SerialNo = "TEST-ASSET-001",
                AssetNo = "TEST-ASSET-001",
                Model = "TEST-MODEL-001",
                Status = BusinessModel.Enums.AssetStatus.Available,
                UserId = null
            });
            // add more test assets here if needed
            dbContext.Assets.Add(new Asset
            {
                SerialNo = "TEST-ASSET-002",
                AssetNo = "TEST-ASSET-002",
                Model = "TEST-MODEL-002",
                Status = BusinessModel.Enums.AssetStatus.Available,
                UserId = null
            });
            dbContext.Assets.Add(new Asset
            {
                SerialNo = "TEST-ASSET-003",
                AssetNo = "TEST-ASSET-003",
                Model = "TEST-MODEL-003",
                Status = BusinessModel.Enums.AssetStatus.Available,
                UserId = null
            });
            dbContext.Assets.Add(new Asset
            {
                SerialNo = "TEST-ASSET-004",
                AssetNo = "TEST-ASSET-004",
                Model = "TEST-MODEL-004",
                Status = BusinessModel.Enums.AssetStatus.Available,
                UserId = null
            }); 
            await dbContext.SaveChangesAsync();
        }
    }
}
