using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Features.Users.Services;
using Yugen.Common.Data.Cqrs;

namespace Poc.Identity.Blazor.Components.Pages.ToDo;

public partial class TenantsToDoItems
{
    private readonly IQueryHandler<GetTenantToDoItemsQuery, List<ToDoItemDto>> _tenantHandler;
    private readonly IUserService _userService;
    private List<ToDoItemDto> tenantToDoItemsDto = [];

    public TenantsToDoItems(
        IQueryHandler<GetTenantToDoItemsQuery, List<ToDoItemDto>> tenantHandler,
        IUserService userService)
    {
        _tenantHandler = tenantHandler;
        _userService = userService;
    }

    protected override async Task OnInitializedAsync()
    {
        var result2 = await _tenantHandler.Handle(new GetTenantToDoItemsQuery(_userService.UserProfileState?.Tenant?.Id), CancellationToken.None);
        if (result2.IsSuccess)
        {
            tenantToDoItemsDto = result2.Value;
        }

        await base.OnInitializedAsync();
    }
}
