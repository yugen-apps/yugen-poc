using Microsoft.EntityFrameworkCore;

namespace Poc.Common.Data;

public class BaseRepository<T> : BaseRepository where T : class
{
	/// <summary>
	/// _dbSet
	/// </summary>
	protected readonly DbSet<T> _dbSet;

	/// <summary>
	/// BaseRepository
	/// </summary>
	/// <param name="context"></param>
	public BaseRepository(DbContext dbContext) : base(dbContext)
	{
		_dbSet = _dbContext.Set<T>();
	}
}
