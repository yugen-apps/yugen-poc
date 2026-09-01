using System;
using System.Threading;
using System.Threading.Tasks;
using Poc.Identity.Domain.Entities;
using Yugen.Common.Persistence;
using Yugen.Common.Persistence.Cqrs;
using Yugen.Common.Core.Models;

namespace Poc.Identity.Application.Features.ToDoItems.Commands;

public sealed record DeleteToDoCommand(Guid Id) : ICommand;

public sealed class DeleteToDoCommandHandler : ICommandHandler<DeleteToDoCommand>
{
    // private IDbContext _dbContext;
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public DeleteToDoCommandHandler(
        // IDbContext dbContext, 
        IApplicationDbContextFactory dbContextFactory)
    {
        // _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result> Handle(DeleteToDoCommand command, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await dbContext
            .DeleteAsync<ToDoItem>(command.Id);

        return result == 0 ?
            Result.Failure("Error") :
            Result.Success();
    }
}