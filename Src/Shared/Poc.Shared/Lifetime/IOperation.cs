using System;

namespace Poc.Shared.Lifetime;

// Source - https://stackoverflow.com/a/38139500
public interface IOperation
{
    Guid OperationId { get; }

    string? TestString { get; set;  }
}

public interface IOperationTransient : IOperation;

public interface IOperationScoped : IOperation;

public interface IOperationSingleton : IOperation;

public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton
{
    public Guid OperationId { get; } = Guid.NewGuid();

    public string? TestString { get; set; }
}
