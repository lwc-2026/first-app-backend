using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class AssetHistoryConfiguration : IEntityTypeConfiguration<AssetHistory>
{
    public void Configure(EntityTypeBuilder<AssetHistory> builder)
    {
        builder.HasOne(ah => ah.Asset)
            .WithMany(a => a.AssetHistories)
            .HasForeignKey(ah => ah.AssetId);

        builder.HasOne(ah => ah.CreatedByUser)
            .WithMany(u => u.CreatedAssetHistories)
            .HasForeignKey(ah => ah.CreatedByUserId);
        
        builder.HasOne(ah => ah.AssignedUser)
            .WithMany(u => u.AssignedAssetHistories)
            .HasForeignKey(ah => ah.AssignedUserId);
    }
}
