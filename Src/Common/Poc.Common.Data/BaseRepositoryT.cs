using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Poc.Common.Data;

public class BaseRepository<T> : BaseRepository, IBaseRepository<T> where T : BaseEntity
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

    public IQueryable<T> Query => DbSet;

    public async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        T? exist = await DbSet.FindAsync(entity.Id);
        if (exist == null)
        {
            return default;
        }

        DbSet.Entry(exist)
            .CurrentValues
            .SetValues(entity);
        return entity;
    }

    public Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<T?> GetAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }
}
