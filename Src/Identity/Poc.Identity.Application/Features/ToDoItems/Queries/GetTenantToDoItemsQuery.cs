using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Domain.Entities;
using Yugen.Common.Persistence;
using Yugen.Common.Persistence.Cqrs;
using ToDoItemMapper = Poc.Identity.Application.Features.ToDoItems.Mappers.ToDoItemMapper;
using Yugen.Common.Core.Models;

namespace Poc.Identity.Application.Features.ToDoItems.Queries;

public sealed record GetTenantToDoItemsQuery(Guid? TenantId) : IQuery<List<ToDoItemDto>>;

public sealed class GetTenantToDoItemsQueryHandler : IQueryHandler<GetTenantToDoItemsQuery, List<ToDoItemDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public GetTenantToDoItemsQueryHandler(IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<List<ToDoItemDto>>> Handle(GetTenantToDoItemsQuery query, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        var tenantsDto = await ToDoItemMapper.ToToDoItemsDto(dbContext
                .Set<ToDoItem>()
                .Where(x => x.TenantId == query.TenantId)
                .AsNoTracking())
            .ToListAsync(cancellationToken);

        return Result.Success(tenantsDto);
    }
}
