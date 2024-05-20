using ActivityManager.Domain.Entities;
using StatusEntity = ActivityManager.Domain.Entities.Status;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace ActivityManager.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Activity> Activities { get; }
    DbSet<Domain.Entities.ActivityType> ActivityTypes { get; }
    DbSet<Job> Jobs { get; }
    DbSet<StatusEntity> Status { get; }
    DbSet<Tag> Tags { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    // Task BulkInsertAsync<TEntity>(IList<TEntity> entities) where TEntity : class;
}

public static class IApplicationDbContextExtensions
{
    public static async Task BulkInsertAsync<TEntity>(this IApplicationDbContext context, IEnumerable<TEntity> entities,
        BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (context is DbContext dbContext)
        {
            await dbContext.BulkInsertAsync(entities, bulkConfig, progress, type, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException("The provided context does not support bulk operations.");
        }
    }
}
