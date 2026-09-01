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

public class AddCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICategoryService _categoryService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AddCommand> _logger;

    public AddCommand(
        IAnsiConsole console,
        IServiceProvider serviceProvider,
        ILogger<AddCommand> logger)
    {
        _console = console;
        _serviceProvider = serviceProvider;
        _logger = logger;

        using var scope = _serviceProvider.CreateScope();
        _categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.WriteLine("Add...");

        var title = _console.Prompt(
            new TextPrompt<string>("Insert Title:"));

        var result = await _categoryService.AddAsync(new() { Title = title }, cancellationToken);

        LogHelper.Log(_console, result);

        return 0;
    }
}
