using Microsoft.EntityFrameworkCore;

namespace Poc.Common.Data;

public class BaseRepository<T> : BaseRepository where T : class
{
    /// <summary>
    /// _dbSet
    /// </summary>
    protected readonly DbSet<T> DbSet;

    /// <summary>
    /// BaseRepository
    /// </summary>
    /// <param name="dbContext"></param>
    public BaseRepository(DbContext dbContext) : base(dbContext)
    {
        DbSet = DbContext.Set<T>();
    }
}
