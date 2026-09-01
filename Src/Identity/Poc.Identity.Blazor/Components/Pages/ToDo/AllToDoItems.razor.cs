using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Identity.Core.Features.Users.Services;
using Yugen.Common.Persistence.Cqrs;

namespace Poc.Identity.Blazor.Components.Pages.ToDo;

public partial class AllToDoItems
{
    private readonly IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>> _allHandler;
    private readonly IUserService _userService;
    private List<ToDoItemDto> allToDoItemsDto = [];

    public AllToDoItems(
        IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>> allHandler,
        IUserService userService)
    {
        _allHandler = allHandler;
        _userService = userService;
    }

    protected override async Task OnInitializedAsync()
    {
        var result = await _allHandler.Handle(new GetAllToDoItemsQuery(), CancellationToken.None);
        if (result.IsSuccess)
        {
            allToDoItemsDto = result.Value;
        }

        await base.OnInitializedAsync();
    }
}
