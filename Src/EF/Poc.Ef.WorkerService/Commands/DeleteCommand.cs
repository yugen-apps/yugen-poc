using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poc.Ef.Application.Contracts.Services;
using Poc.Ef.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.WorkerService.Commands;

public class DeleteCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICategoryService _categoryService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeleteCommand> _logger;

    public DeleteCommand(
        IAnsiConsole console,
        IServiceProvider serviceProvider,
        ILogger<DeleteCommand> logger)
    {
        _console = console;
        _serviceProvider = serviceProvider;
        _logger = logger;

        using var scope = _serviceProvider.CreateScope();
        _categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.WriteLine("Update...");

        var id = _console.Prompt(
            new TextPrompt<int>("Insert Id:"));

        var result = await _categoryService.DeleteAsync(id, cancellationToken);

        LogHelper.Log(_console, result);

        return 0;
    }
}
