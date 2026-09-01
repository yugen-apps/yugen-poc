using Microsoft.EntityFrameworkCore;

namespace Poc.Common.Data;

public class BaseRepository
{
    /// <summary>
    /// _dbContext
    /// </summary>
    protected readonly DbContext DbContext;

    /// <summary>
    /// BaseRepository
    /// </summary>
    /// <param name="dbContext"></param>
    public BaseRepository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public bool CanConnect() => DbContext.Database.CanConnect();
}
