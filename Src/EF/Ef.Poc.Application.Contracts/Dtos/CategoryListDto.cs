namespace Ef.Poc.Application.Contracts.Dtos;

//[Serializable]
public class CategoryListDto
{
    public int Id { get; set; }

    public required string Title { get; set; }
}
