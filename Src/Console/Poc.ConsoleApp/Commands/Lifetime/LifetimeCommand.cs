using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.ConsoleApp.Commands.Lifetime;

public class LifetimeCommand : AsyncCommand
{
    private readonly IOperationTransient _transientOperation;
    private readonly IOperationScoped _scopedOperation;
    private readonly IOperationSingleton _singletonOperation;
    private readonly OperationService _operationService;
    private readonly IAnsiConsole _console;
    private readonly IServiceProvider _serviceProvider;

    public LifetimeCommand(
        IOperationTransient transientOperation,
        IOperationScoped scopedOperation,
        IOperationSingleton singletonOperation,
        OperationService operationService,
        IAnsiConsole console,
        IServiceProvider serviceProvider)
    {
        _transientOperation = transientOperation;
        _scopedOperation = scopedOperation;
        _singletonOperation = singletonOperation;
        _operationService = operationService;
        _console = console;
        _serviceProvider = serviceProvider;

        //using var scope = _serviceProvider.CreateScope();
        //_categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.WriteLine($"_transientOperation: {_transientOperation.OperationId}");
        _console.WriteLine($"_scopedOperation: {_scopedOperation.OperationId}");
        _console.WriteLine($"_singletonOperation: {_singletonOperation.OperationId}");
        //_console.WriteLine($"_singletonInstanceOperation: {_singletonInstanceOperation.OperationId}");

        _operationService.Print();

        return 0;
    }
}