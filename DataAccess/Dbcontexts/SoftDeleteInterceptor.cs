using System;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess.Dbcontexts;

public class SoftDeleteInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider = timeProvider;
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        HandleSoftDelete(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        HandleSoftDelete(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleSoftDelete(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = _timeProvider.GetUtcNow().UtcDateTime;
            }
        }
    }
}
