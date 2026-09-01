using System.Linq;
using System.Threading.Tasks;

namespace Poc.Common.Data;

public interface IBaseRepository<T> where T : BaseEntity
{
    IQueryable<T> Query { get; }

    Task<T> AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T?> GetAsync(int id);
    Task<T?> UpdateAsync(T entity);
}