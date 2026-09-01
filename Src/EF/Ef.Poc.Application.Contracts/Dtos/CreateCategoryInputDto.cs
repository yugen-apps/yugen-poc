using Ef.Poc.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace Ef.Poc.Application.Contracts.Dtos;

//[Serializable]
public class CreateCategoryInputDto
{
    [StringLength(AppConstants.MaxNameLength)]
    public required string Title { get; set; }
}
