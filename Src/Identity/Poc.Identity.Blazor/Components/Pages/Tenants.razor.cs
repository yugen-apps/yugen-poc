using Poc.Identity.Application.Features.Tenants.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;
using Yugen.Common.Data.Commands;

namespace Poc.Identity.Blazor.Components.Pages;

public partial class Tenants
{
    private readonly IQueryHandler<GetAllTenantsQuery, List<TenantDto>> _handler;
    private List<TenantDto> tenantsDto = [];

    public Tenants(IQueryHandler<GetAllTenantsQuery, List<TenantDto>> handler)
    {
        _handler = handler;
    }

    protected override async Task OnInitializedAsync()
    {
        var result = await _handler.Handle(new GetAllTenantsQuery(), CancellationToken.None);
        if (result.IsSuccess)
        {
            tenantsDto = result.Value;
        }

        await base.OnInitializedAsync();
    }
}
