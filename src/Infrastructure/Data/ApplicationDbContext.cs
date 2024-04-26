using System.Reflection;
using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Domain.Entities;
using ActivityManager.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ActivityManager.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<ActivityType> ActivityTypes => Set<ActivityType>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Status> Status => Set<Status>();
    public DbSet<Tag> Tags => Set<Tag>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    // public async Task BulkInsertAsync<TEntity>(IList<TEntity> entities) where TEntity : class
    // {
    //     await BulkInsertAsync<TEntity>(entities);
    // }
}
