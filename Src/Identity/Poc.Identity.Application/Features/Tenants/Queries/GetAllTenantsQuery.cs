using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;
using Yugen.Common.Blazor.Account.Features.Tenants.Mappers;
using Yugen.Common.Data;
using Yugen.Common.Data.Cqrs;
using Yugen.Common.Models;

namespace Poc.Identity.Application.Features.Tenants.Queries;

public sealed record GetAllTenantsQuery : IQuery<List<TenantDto>>;

public sealed class GetAllTenantsQueryHandler : IQueryHandler<GetAllTenantsQuery, List<TenantDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public GetAllTenantsQueryHandler(IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<List<TenantDto>>> Handle(GetAllTenantsQuery query, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        var tenantsDto = await dbContext
            .Set<Tenant>()
            .AsNoTracking()
            .ToTenantsDto()
            .ToListAsync(cancellationToken);

        return Result.Success(tenantsDto);
    }
}
