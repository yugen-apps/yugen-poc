using Microsoft.Extensions.Logging;
using System;

namespace Poc.ConsoleApp.Commands.Lifetime;

// Source - https://stackoverflow.com/a/38139500
public interface IOperation
{
    Guid OperationId { get; }
}

public interface IOperationTransient : IOperation;

public interface IOperationScoped : IOperation;

public interface IOperationSingleton : IOperation;

public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton
{
    public Guid OperationId { get; } = Guid.NewGuid();
}

public class OperationService
{
    public IOperationTransient TransientOperation { get; }
    public IOperationScoped ScopedOperation { get; }
    public IOperationSingleton SingletonOperation { get; }
    private readonly ILogger<OperationService> _logger;

    public OperationService(
        IOperationTransient transientOperation,
        IOperationScoped scopedOperation,
        IOperationSingleton singletonOperation,
        ILogger<OperationService> logger)
    {
        TransientOperation = transientOperation;
        ScopedOperation = scopedOperation;
        SingletonOperation = singletonOperation;
        _logger = logger;
    }

    public void Print()
    {
        _logger.LogInformation($"TransientOperation: {TransientOperation.OperationId}");
        _logger.LogInformation($"ScopedOperation: {ScopedOperation.OperationId}");
        _logger.LogInformation($"SingletonOperation: {SingletonOperation.OperationId}");
    }
}