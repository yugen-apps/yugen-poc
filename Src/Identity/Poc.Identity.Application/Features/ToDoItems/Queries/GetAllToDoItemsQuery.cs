using Microsoft.EntityFrameworkCore;
using Poc.Ef.Application.Mappers;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Identity.Application.Features.ToDoItems.Queries;

public sealed record GetAllToDoItemsQuery() : IQuery<List<ToDoItemDto>>;

public sealed class GetAllToDoItemsQueryHandler : IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public GetAllToDoItemsQueryHandler(IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<List<ToDoItemDto>>> Handle(GetAllToDoItemsQuery query, CancellationToken cancellationToken)
    {
        await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        List<ToDoItemDto> tenantsDto = await _dbContext
            .Set<ToDoItem>()
            .Select(x => x.ToToDoItemDto())
            .ToListAsync(cancellationToken);

        return Result.Success(tenantsDto);
    }
}
