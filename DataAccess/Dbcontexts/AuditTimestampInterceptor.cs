using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess.Dbcontexts;

public class AuditTimestampInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateTimestamps(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateTimestamps(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateTimestamps(DbContext? context)
    {
        if (context == null) return;

        var now = timeProvider.GetUtcNow().UtcDateTime;

        foreach (var entry in context.ChangeTracker.Entries<IAuditable>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = now;
                entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = now;
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = now;
            }
        }
    }
}
