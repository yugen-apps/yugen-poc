using Poc.Common.Data;
using Poc.Ef.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poc.Ef.Application.Contracts.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<List<Category>> GetAllAsync(int? parentId = null);

    Task<List<Category>> GetByTitleAsync(string title);

    Task<bool> ExistsAsync(string title);
}