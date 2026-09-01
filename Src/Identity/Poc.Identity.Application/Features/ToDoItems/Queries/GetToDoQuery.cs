using System;
using System.Threading;
using System.Threading.Tasks;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Mappers;
using Poc.Identity.Domain.Entities;
using Yugen.Common.Persistence;
using Yugen.Common.Persistence.Cqrs;
using Yugen.Common.Core.Models;

namespace Poc.Identity.Application.Features.ToDoItems.Queries;

public sealed record GetToDoQuery(Guid Id) : IQuery<ToDoItemDto>;

public sealed class GetToDoQueryHandler : IQueryHandler<GetToDoQuery, ToDoItemDto>
{
    // private IDbContext _dbContext;
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public GetToDoQueryHandler(
        // IDbContext dbContext, 
        IApplicationDbContextFactory dbContextFactory)
    {
        // _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<ToDoItemDto>> Handle(GetToDoQuery command, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await dbContext
            .GetBydIdAsync<ToDoItem>(command.Id);

        //TodoItem? todoItem = await _dbContext
        //	.Set<TodoItem>()
        //	.SingleOrDefaultAsync(
        //		t => t.Id == command.id,
        //		cancellationToken);

        return result is null ?
            Result.Failure<ToDoItemDto>("Error") :
            Result.Success(result.ToToDoItemDto());
    }
}