using Poc.Ef.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace Poc.Ef.Application.Contracts.Dtos;

//[Serializable]
public class SubCategoryDto
{
    public int? Id { get; set; }

    [StringLength(AppConstants.MaxNameLength)]
    public required string Title { get; set; }
}
