using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;
using Yugen.Common.Blazor.Account.Features.Tenants.Mappers;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Identity.Application.Features.Tenants.Queries;

public sealed record GetAllTenantsQuery() : IQuery<List<TenantDto>>;

public sealed class GetAllTenantsQueryHandler : IQueryHandler<GetAllTenantsQuery, List<TenantDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public GetAllTenantsQueryHandler(IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<List<TenantDto>>> Handle(GetAllTenantsQuery query, CancellationToken cancellationToken)
    {
        await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        List<TenantDto> tenantsDto = await _dbContext
            .Set<Tenant>()
            .Select(x => x.ToTenantDto())
            .ToListAsync(cancellationToken);

        return Result.Success(tenantsDto);
    }
}
