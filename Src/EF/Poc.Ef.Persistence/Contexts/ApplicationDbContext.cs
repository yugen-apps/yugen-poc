using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Poc.Common.Data;
using Poc.Common.Data.Entities;
using Poc.Ef.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Contexts;

/// <summary>
/// Represents the applications database context.
/// </summary>
public sealed class ApplicationDbContext : DbContext, IDbContext
{
	public DbSet<TodoItem> TodoItems => Set<TodoItem>();

	/// <summary>
	/// Initializes a new instance of the <see cref="RallySimulatorDbContext"/> class.
	/// </summary>
	/// <param name="options">The database context options.</param>
	/// <param name="dateTime">The current date and time.</param>
	/// <param name="publisher">The publisher.</param>
	public ApplicationDbContext(DbContextOptions options)
		: base(options)
	{
	}

	/// <inheritdoc />
	public new DbSet<TEntity> Set<TEntity>()
		where TEntity : BaseEntity =>
			base.Set<TEntity>();

	/// <inheritdoc />
	public ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity)
		where TEntity : BaseEntity =>
			Set<TEntity>()
			.AddAsync(entity);

	/// <inheritdoc />
	public async Task<TEntity?> GetBydIdAsync<TEntity>(int id)
		where TEntity : BaseEntity
	{
		if (id <= 0)
		{
			return default;
		}

		return await Set<TEntity>()
			.FirstOrDefaultAsync(e => e.Id == id);
	}

	/// <inheritdoc />
	public Task<int> DeleteAsync<TEntity>(int id)
		where TEntity : BaseEntity =>
			Set<TEntity>()
			.Where(x => x.Id == id)
			.ExecuteDeleteAsync();

	/// <summary>
	/// Saves all of the pending changes in the unit of work.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The number of entities that have been saved.</returns>
	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		//DateTime utcNow = _dateTime.UtcNow;
		DateTime utcNow = DateTime.UtcNow;

		//UpdateAuditableEntities(utcNow);

		return await base.SaveChangesAsync(cancellationToken);
	}

	public Task<bool> CanConnectAsync() => 
		Database.CanConnectAsync();

	/// <inheritdoc />
	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
	//    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

	//    //modelBuilder.ApplyUtcDateTimeConverter();

	//    base.OnModelCreating(modelBuilder);
	//}

	/// <summary>
	/// Updates the entities implementing <see cref="IAuditableEntity"/> interface.
	/// </summary>
	/// <param name="utcNow">The current date and time in UTC format.</param>
	//private void UpdateAuditableEntities(DateTime utcNow)
	//{
	//    foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
	//    {
	//        if (entityEntry.State == EntityState.Added)
	//        {
	//            entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = utcNow;
	//        }

	//        if (entityEntry.State == EntityState.Modified)
	//        {
	//            entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = utcNow;
	//        }
	//    }
	//}
}