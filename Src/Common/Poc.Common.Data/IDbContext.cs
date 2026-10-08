using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Poc.Common.Data.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Common.Data;

public interface IDbContext
{
	/// <summary>
	/// Gets the database set for the specified entity type.
	/// </summary>
	/// <typeparam name="TEntity">The entity type.</typeparam>
	/// <returns>The database set for the specified entity type.</returns>
	DbSet<TEntity> Set<TEntity>()
		where TEntity : BaseEntity;

	/// <summary>
	/// Inserts the specified entity into the database.
	/// </summary>
	/// <typeparam name="TEntity">The entity type.</typeparam>
	/// <param name="entity">The entity to be inserted into the database.</param>
	ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity)
		where TEntity : BaseEntity;

	/// <summary>
	/// Gets the entity with the specified identifier.
	/// </summary>
	/// <typeparam name="TEntity">The entity type.</typeparam>
	/// <param name="id">The entity identifier.</param>
	/// <returns>The maybe instance that may contain the <typeparamref name="TEntity"/> with the specified identifier.</returns>
	Task<TEntity?> GetBydIdAsync<TEntity>(int id)
		where TEntity : BaseEntity;

	/// <summary>
	/// Removes the specified entity from the database.
	/// </summary>
	/// <typeparam name="TEntity">The entity type.</typeparam>
	/// <param name="entity">The entity to be removed from the database.</param>
	Task<int> DeleteAsync<TEntity>(int id) where TEntity : BaseEntity;

	/// <summary>
	/// Saves all of the pending changes in the unit of work.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The number of entities that have been saved.</returns>
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

	Task<bool> CanConnectAsync();
}