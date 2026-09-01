using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Yugen.Common.Data;
using Yugen.Common.Data.Entities;
using Poc.Ef.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Contexts;

/// <summary>
/// Represents the applications database dbContext.
/// </summary>
public sealed class ApplicationDbContext : DbContext, IDbContext
{
	public DbSet<TodoItem> TodoItems => Set<TodoItem>();

	/// <summary>
	/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
	/// </summary>
	/// <param name="options">The database dbContext options.</param>
	public ApplicationDbContext(DbContextOptions options)
		: base(options)
	{
	}

	/// <inheritdoc />
	public new DbSet<TEntity> Set<TEntity>()
		where TEntity : BaseEntity =>
			base.Set<TEntity>();

	//     public IQueryable<T> Query => DbSet;
	
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
			return null;
		}

		return await Set<TEntity>()
			.FirstOrDefaultAsync(e => e.Id == id);
	}
	
	// public async Task<TEntity?> GetBydIdAsync<TEntity>(int id)
	// 	where TEntity : BaseEntity
	// {
	// 	if (id <= 0)
	// 	{
	// 		return null;
	// 	}
	//
	// 	return await Set<TEntity>()
	// 		.FindAsync(id);
	// }

	/// <inheritdoc />
	public Task<int> DeleteAsync<TEntity>(int id)
		where TEntity : BaseEntity =>
			Set<TEntity>()
			.Where(x => x.Id == id)
			.ExecuteDeleteAsync();

	// public Task DeleteAsync<TEntity>(TEntity entity)
	// {
	// 	Set<TEntity>().Remove(entity);
	// 	return Task.CompletedTask;
	// }
	
	//     public async Task<T?> UpdateAsync(T entity)
//     {
//         T? exist = await DbSet.FindAsync(entity.Id);
//         if (exist == null)
//         {
//             return default;
//         }
//
//         DbSet.Entry(exist)
//             .CurrentValues
//             .SetValues(entity);
//         return entity;
//     }
	
	/// <summary>
	/// Saves all of the pending changes in the unit of work.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The number of entities that have been saved.</returns>
	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		//DateTime utcNow = _dateTime.UtcNow;
		var utcNow = DateTime.UtcNow;

		//UpdateAuditableEntities(utcNow);

		return await base.SaveChangesAsync(cancellationToken);
	}

	public Task<bool> CanConnectAsync() => 
		Database.CanConnectAsync();

//
//     public Task<bool> ExistsAsync(string title)
//     {
//         return Query.AnyAsync(x => x.Title == title);
//     }

	/// <inheritdoc />
	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
	//    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	// OR
	//         Map(modelBuilder.Entity<Category>());

	// builder.ApplyGlobalFilters<ISoftDelete>(s => s.DeletedAt == null);
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
	
	//     private static void Map(EntityTypeBuilder<Category> entity)
//     {
//         entity.HasKey(x => x.Id);
//
//         //entity.Property(e => e.Id).ValueGeneratedOnAdd();
//
//         //entity.HasQueryFilter(x => !x.IsDeleted);
//
//         entity.Property(x => x.Title)
//             .IsRequired()
//             .HasMaxLength(512);
//
//         //entity.HasMany<Category>().WithOne().HasForeignKey(x => x.ParentId);
//
//         entity.HasIndex(x => x.Title);
//     }
//
//     public Task Rollback()
//     {
//         _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
//         return Task.CompletedTask;
//     }
//
//     public async Task<int> Save(CancellationToken cancellationToken)
//     {
//         return await _dbContext.SaveChangesAsync(cancellationToken);
//     }
}