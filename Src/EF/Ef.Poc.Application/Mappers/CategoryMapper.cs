using Ef.Poc.Application.Contracts.Dtos;
using Ef.Poc.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Ef.Poc.Infrastructure;

[Mapper]
public static partial class CategoryMapper
{
    public static partial CategoryListDto ToCategoryListDto(this Category category);

    [MapperIgnoreTarget(nameof(CategoryDto.SubCategories))]
    public static partial CategoryDto ToCategoryDto(this Category category);

    [MapperIgnoreTarget(nameof(Category.Id))]
    public static partial Category ToCategory(this CreateCategoryInputDto category);

    public static partial Category ToCategory(this UpdateCategoryInputDto category, int id);
}