using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataAccess.Configurations;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);

        builder.HasOne(asset => asset.User)
            .WithMany(user => user.Assets)
            .HasForeignKey(asset => asset.UserId);
    }
}
