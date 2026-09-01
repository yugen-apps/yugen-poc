using Ef.Poc.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Ef.Poc.Application.Contracts.Repositories;

public interface IBaseRepository<T> where T : class, IEntity
{
    IQueryable<T> Query { get; }

    Task<T?> GetAsync(int id);

    Task<T> AddAsync(T entity);

    Task<T?> UpdateAsync(T entity);

    Task DeleteAsync(T entity);
}
