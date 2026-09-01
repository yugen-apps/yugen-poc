using Poc.Common.Data;

namespace Poc.Ef.Domain.Entities;

public class Category : BaseEntity
{
    public required string Title { get; set; }
}
