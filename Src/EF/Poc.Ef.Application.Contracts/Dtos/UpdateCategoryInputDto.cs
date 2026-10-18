using Poc.Ef.Domain.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Poc.Ef.Application.Contracts.Dtos;

//[Serializable]
public class UpdateCategoryInputDto
{
    [StringLength(AppConstants.MaxNameLength)]
    public required string Title { get; set; }

    public List<SubCategoryDto> SubCategories { get; set; } = [];
}
