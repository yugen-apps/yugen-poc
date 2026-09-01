using System;
using System.Threading;
using System.Threading.Tasks;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Mappers;
using Poc.Identity.Domain.Entities;
using Yugen.Common.Persistence;
using Yugen.Common.Persistence.Cqrs;
using Yugen.Common.Core.Models;

namespace Poc.Identity.Application.Features.ToDoItems.Commands;

public sealed record UpdateToDoCommand(Guid Id, string Title) : ICommand<ToDoItemDto>;

public sealed class UpdateToDoCommandHandler : ICommandHandler<UpdateToDoCommand, ToDoItemDto>
{
    // private IDbContext _dbContext;
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public UpdateToDoCommandHandler(
        // IDbContext dbContext, 
        IApplicationDbContextFactory dbContextFactory)
    {
        // _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<ToDoItemDto>> Handle(UpdateToDoCommand command, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        var todoItem = await dbContext
            .GetBydIdAsync<ToDoItem>(command.Id);

        if (todoItem is null)
        {
            return Result.Failure<ToDoItemDto>("Error");
        }

        todoItem.Title = command.Title;

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result == 0 ?
            Result.Failure<ToDoItemDto>("Error") :
            Result.Success(todoItem.ToToDoItemDto());
    }
}