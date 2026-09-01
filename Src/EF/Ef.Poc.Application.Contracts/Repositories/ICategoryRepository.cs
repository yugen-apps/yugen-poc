using Ef.Poc.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ef.Poc.Application.Contracts.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<List<Category>> GetAllAsync(int? parentId = null);

    Task<List<Category>> GetByTitleAsync(string title);

    Task<bool> ExistsAsync(string title);
}