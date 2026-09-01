using System.Threading;
using System.Threading.Tasks;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Mappers;
using Poc.Identity.Domain.Entities;
using Yugen.Common.Persistence;
using Yugen.Common.Persistence.Cqrs;
using Yugen.Common.Core.Models;

namespace Poc.Identity.Application.Features.ToDoItems.Commands;

public sealed record AddToDoCommand(string Title) : ICommand<ToDoItemDto>;

public sealed class AddToDoCommandHandler : ICommandHandler<AddToDoCommand, ToDoItemDto>
{
    // private IDbContext _dbContext;
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public AddToDoCommandHandler(
        // IDbContext dbContext, 
        IApplicationDbContextFactory dbContextFactory)
    {
        // _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<ToDoItemDto>> Handle(AddToDoCommand command, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        ToDoItem todoItem = new() { Title = command.Title };

        var todoItemEntityEntry = await dbContext
            .AddAsync(todoItem);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result == 0 ?
            Result.Failure<ToDoItemDto>("Error") :
            Result.Success(todoItemEntityEntry.Entity.ToToDoItemDto());
    }
}