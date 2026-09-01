using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yugen.Common.Identity.Domain.Entities;
using Yugen.Common.Persistence;
using Yugen.Common.Persistence.Cqrs;
using Yugen.Common.Identity.Core.Features.Tenants.DTOs;
using TenantMapper = Yugen.Common.Identity.Core.Features.Tenants.Mappers.TenantMapper;
using Yugen.Common.Core.Models;

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

        var tenantsDto = await EntityFrameworkQueryableExtensions
            .ToListAsync<TenantDto>(TenantMapper.ToTenantsDto(dbContext
                    .Set<Tenant>()
                    .AsNoTracking()), cancellationToken);

        return Result.Success(tenantsDto);
    }
}
