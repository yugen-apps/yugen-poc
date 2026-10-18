using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Data.Commands;

namespace Poc.Identity.Blazor.Components.Pages;

public partial class ToDoItems
{
    private readonly IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>> _handler;
    private List<ToDoItemDto> toDoItemsDto = [];

    public ToDoItems(IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>> handler)
    {
        _handler = handler;
    }

    protected override async Task OnInitializedAsync()
    {
        var result = await _handler.Handle(new GetAllToDoItemsQuery(), CancellationToken.None);
        if (result.IsSuccess)
        {
            toDoItemsDto = result.Value;
        }

        await base.OnInitializedAsync();
    }
}
