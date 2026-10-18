using Microsoft.Extensions.Logging;

namespace Poc.Shared.Lifetime;

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
        _logger.LogInformation("TransientOperation: {OperationId}", TransientOperation.OperationId);
        _logger.LogInformation("ScopedOperation: {OperationId}", ScopedOperation.OperationId);
        _logger.LogInformation("SingletonOperation: {OperationId}", SingletonOperation.OperationId);
    }
}