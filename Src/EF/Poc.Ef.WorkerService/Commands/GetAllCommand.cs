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

public class GetAllCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICategoryService _categoryService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GetAllCommand> _logger;

    public GetAllCommand(
        IAnsiConsole console,
        IServiceProvider serviceProvider,
        ILogger<GetAllCommand> logger)
    {
        _console = console;
        _serviceProvider = serviceProvider;
        _logger = logger;

        using var scope = _serviceProvider.CreateScope();
        _categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.WriteLine("GetAll...");

        var result = await _categoryService.GetAllAsync();

        LogHelper.Log(_console, result);

        return 0;
    }
}
