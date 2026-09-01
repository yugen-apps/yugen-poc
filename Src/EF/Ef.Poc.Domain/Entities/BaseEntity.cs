using Ef.Poc.Domain.Interfaces;

namespace Ef.Poc.Domain.Entities;

public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }
}
