using Poc.Common.Data;
using Poc.Ef.Application.Contracts.Dtos;
using Poc.Ef.Application.Contracts.Repositories;
using Poc.Ef.Application.Contracts.Services;
using Poc.Ef.Domain.Shared;
using Poc.Ef.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _repository;

    public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<Result<List<CategoryListDto>>> GetAllAsync()
    {
        var categories = _repository
                .Query
                .Select(x => x.ToCategoryListDto())
                .ToList();

        return categories == null ?
            await Result<List<CategoryListDto>>.FailureAsync() :
            await Result<List<CategoryListDto>>.SuccessAsync(categories);
    }

    public async Task<Result<CategoryDto>> GetAsync(int id)
    {
        var category = await _repository.GetAsync(id);

        var categoryDto = category?.ToCategoryDto();

        return categoryDto == null ?
            await Result<CategoryDto>.FailureAsync() :
            await Result<CategoryDto>.SuccessAsync(categoryDto);

        //var subCategories = await  _repository.ListAsync(parentId: id);
        //return new CategoryDto
        //{
        //    Id = category.Id,
        //    Name = category.Title,
        //    //SubCategories = subCategories.Select(c => new CategoryDto
        //    //{
        //    //    Id = c.Id,
        //    //    Title = c.Title
        //    //}).ToList()
        //};
    }

    //public async Task<Result<CategoryDto>> GetByTitleAsync(string title)
    //{
    //    var category = await _repository.GetByTitleAsync(title);

    //    var categoryDto = category?.ToCategoryDto();

    //    return categoryDto == null ?
    //        await Result<CategoryDto>.FailureAsync() :
    //        await Result<CategoryDto>.SuccessAsync(categoryDto);
    //}

    public async Task<Result<CategoryDto>> AddAsync(CreateCategoryInputDto input, CancellationToken cancellationToken)
    {
        //var exists = await _repository.ExistsAsync(input.Title);

        //if (exists)
        //{
        //    return null;
        //    // TODO
        //    // throw an exception
        //    // {error: "Category already exists"}
        //}

        var category = await _repository.AddAsync(input.ToCategory());

        await _unitOfWork.Save(cancellationToken);

        var categoryDto = category?.ToCategoryDto();

        return categoryDto == null ?
            await Result<CategoryDto>.FailureAsync() :
            await Result<CategoryDto>.SuccessAsync(categoryDto);
    }

    public async Task<Result<int>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(id);
        if (category == null)
        {
            return await Result<int>.FailureAsync();
        }

        await _repository.DeleteAsync(category);

        await _unitOfWork.Save(cancellationToken);

        return await Result<int>.SuccessAsync();
    }

    public async Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryInputDto input, CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(id);
        if (category == null)
        {
            return await Result<CategoryDto>.FailureAsync();
        }

        category = input.ToCategory(id);

        await _repository.UpdateAsync(category);

        await _unitOfWork.Save(cancellationToken);

        var categoryDto = category?.ToCategoryDto();

        return categoryDto == null ?
            await Result<CategoryDto>.FailureAsync() :
            await Result<CategoryDto>.SuccessAsync(categoryDto);

        //if (!string.Equals(category.Title, input.Title, StringComparison.OrdinalIgnoreCase))
        //{
        //    var exists = await _repository.ExistsAsync(input.Title);

        //    if (exists)
        //    {
        //        throw new ApplicationException($"There is already a category with the title {input.Title}");
        //    }

        //    category.SetTitle(input.Title);
        //    await _repository.UpdateAsync(category);
        //}

        //var subCategories = await _repository.ListAsync(category.Id);

        //// deleted sub categories
        //var existingSubCategoryIds = subCategories.Select(x => x.Id).Distinct().ToList();
        //var inputSubCategoryIds = input.SubCategories.Select(x => x.Id).Distinct().ToList();
        //var deletedSubCategoryIds = existingSubCategoryIds.Where(x => !inputSubCategoryIds.Contains(x)).ToList();

        //foreach (var deletedSubCategoryId in deletedSubCategoryIds)
        //{
        //    await _repository.DeleteAsync(subCategories.First(x => x.Id == deletedSubCategoryId));
        //}

        //// newly added sub categories
        //var newAddedSubCategories = input.SubCategories.Where(x => x.Id == null);

        //foreach (var newAddedSubCategory in newAddedSubCategories)
        //{
        //    var newSubCategory = await _repository.CreateAsync(new Category(newAddedSubCategory.Title));
        //}
    }
}
