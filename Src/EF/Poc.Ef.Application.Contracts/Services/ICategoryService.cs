using Poc.Ef.Application.Contracts.Dtos;
using Poc.Ef.Domain.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Application.Contracts.Services;

public interface ICategoryService
{
    Task<Result<List<CategoryListDto>>> GetAllAsync();

    Task<Result<CategoryDto>> GetAsync(int id);

    Task<Result<CategoryDto>> AddAsync(CreateCategoryInputDto input, CancellationToken cancellationToken);

    Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryInputDto input, CancellationToken cancellationToken);

    Task<Result<int>> DeleteAsync(int id, CancellationToken cancellationToken);
}