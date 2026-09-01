using System.Collections.Generic;

namespace Ef.Poc.Application.Contracts.Dtos;

//[Serializable]
public class CategoryDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public List<CategoryDto> SubCategories { get; set; } = [];
}
