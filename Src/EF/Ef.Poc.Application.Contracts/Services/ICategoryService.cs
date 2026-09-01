using Ef.Poc.Application.Contracts.Dtos;
using Ef.Poc.Domain.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ef.Poc.Application.Contracts.Services;

public interface ICategoryService
{
    Task<Result<List<CategoryListDto>>> GetAllAsync();

    Task<Result<CategoryDto>> GetAsync(int id);

    Task<Result<CategoryDto>> AddAsync(CreateCategoryInputDto input, CancellationToken cancellationToken);

    Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryInputDto input, CancellationToken cancellationToken);

    Task<Result<int>> DeleteAsync(int id, CancellationToken cancellationToken);
}