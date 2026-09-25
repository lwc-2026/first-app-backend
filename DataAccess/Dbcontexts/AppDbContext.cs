using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Dbcontexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<AssetHistory> AssetHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // use IignoreQueryFilters in EF Core to automatically filter out soft-deleted entities
        modelBuilder.Entity<AppUser>()
            .HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Asset>()
            .HasQueryFilter(a => !a.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }

}