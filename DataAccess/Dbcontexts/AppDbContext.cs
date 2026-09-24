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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasQueryFilter(u => u.DeletedAt == null);
        modelBuilder.Entity<RefreshToken>()
            .HasQueryFilter(rt => rt.DeletedAt == null);
        modelBuilder.Entity<AuditLog>()
            .HasQueryFilter(al => al.DeletedAt == null);
        modelBuilder.Entity<Asset>()
            .HasQueryFilter(a => a.DeletedAt == null);
        base.OnModelCreating(modelBuilder);
    }

}