using Poc.Ef.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace Poc.Ef.Application.Contracts.Dtos;

//[Serializable]
public class CreateCategoryInputDto
{
    [StringLength(AppConstants.MaxNameLength)]
    public required string Title { get; set; }
}
