using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Poc.Identity.Domain.Entities;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Data;
using Yugen.Common.Data.Entities;

namespace Poc.Identity.Persistence.Contexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantUser> TenantUsers { get; set; }
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public Task<bool> CanConnectAsync() => Database
        .CanConnectAsync();

    public Task<bool> EnsureCreatedAsync() => Database
        .EnsureCreatedAsync();

    public Task MigrateAsync() => Database
        .MigrateAsync();

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : BaseEntity => base.Set<TEntity>();

    public ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity)
        where TEntity : BaseEntity => Set<TEntity>()
            .AddAsync(entity);

    public Task<int> DeleteAsync<TEntity>(Guid id)
        where TEntity : BaseEntity => Set<TEntity>()
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

    public async Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
        where TEntity : BaseEntity
    {
        if (id == Guid.Empty)
        {
            return null;
        }

        return await Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                await SeedAsync();
            })
            .UseSeeding(async (context, _) =>
            {
                await SeedAsync();
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //ApplicationUserConfiguration.Map(modelBuilder.Entity<ApplicationUser>());

        base.OnModelCreating(modelBuilder);
    }

    private async Task SeedAsync()
    {
        await SeedTenantsAsync();
    }

    private async Task SeedTenantsAsync()
    {
        if (await Tenants.AnyAsync())
        {
            return;
        }

        var tenants = new[]
        {
            new Tenant { Name = "Main" },
            new Tenant { Name = "Europe" }
        };

        await Tenants.AddRangeAsync(tenants);
        await SaveChangesAsync();
    }
}
