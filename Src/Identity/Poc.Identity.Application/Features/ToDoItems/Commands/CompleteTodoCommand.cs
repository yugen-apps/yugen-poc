using System;
using System.Threading;
using System.Threading.Tasks;
using Poc.Identity.Domain.Entities;
using Yugen.Common.Data;
using Yugen.Common.Data.Cqrs;
using Yugen.Common.Models;

namespace Poc.Identity.Application.Features.ToDoItems.Commands;

public sealed record CompleteTodoCommand(Guid TodoItemId) : ICommand;

public sealed class CompleteTodoCommandHandler : ICommandHandler<CompleteTodoCommand>
{
    // private IDbContext _dbContext;
    private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IDateTimeProvider dateTimeProvider;

    public CompleteTodoCommandHandler(
        // IDbContext dbContext, 
        IApplicationDbContextFactory dbContextFactory)
    {
        // _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result> Handle(CompleteTodoCommand command, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        var todoItem = await dbContext
            .GetBydIdAsync<ToDoItem>(command.TodoItemId);

        if (todoItem is null)
        {
            return Result.Failure("Error");
        }

        todoItem.IsCompleted = true;
        todoItem.CompletedAt = DateTime.UtcNow;
        //todoItem.CompletedAt = dateTimeProvider.UtcNow;

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result == 0 ?
            Result.Failure("Error") :
            Result.Success();
    }
}