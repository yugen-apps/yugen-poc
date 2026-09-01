using Microsoft.EntityFrameworkCore;

namespace Poc.Common.Data;

public class BaseRepository
{
	/// <summary>
	/// _dbContext
	/// </summary>
	protected readonly DbContext _dbContext;

	/// <summary>
	/// BaseRepository
	/// </summary>
	/// <param name="context"></param>
	public BaseRepository(DbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public bool CanConnect() => _dbContext.Database.CanConnect();
}
