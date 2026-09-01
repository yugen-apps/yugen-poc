using Microsoft.Extensions.Logging;
using Poc.Identity.Application.Features.ToDoItems.Commands;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Persistence.Cqrs;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Identity.WorkerService.Commands;

public class AddToDo : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICommandHandler<AddToDoCommand, ToDoItemDto> _handler;
    private readonly ILogger<AddToDo> _logger;

    public AddToDo(
        IAnsiConsole console,
        ICommandHandler<AddToDoCommand, ToDoItemDto> handler,
        ILogger<AddToDo> logger)
    {
        _console = console;
        _handler = handler;
        _logger = logger;

        //using var scope = _serviceProvider.CreateScope();
        //_categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
    }

    public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
    {
        _console.WriteLine("AddToDo...");

        var title = _console.Prompt(
            new TextPrompt<string>("Insert Title:"));

        AddToDoCommand command = new(title);

        var result = await _handler.Handle(command, cancellationToken);
        if (result.IsSuccess)
        {
            LogHelper.Log(_console, result);
        }

        _console.PressAnyKey();

        return 0;
    }
}
