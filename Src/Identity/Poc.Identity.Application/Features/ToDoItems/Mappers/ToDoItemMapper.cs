using System.Linq;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Poc.Identity.Application.Features.ToDoItems.Mappers;

[Mapper]
public static partial class ToDoItemMapper
{
    [MapperIgnoreSource(nameof(ToDoItem.Tenant))]
    public static partial ToDoItemDto ToToDoItemDto(this ToDoItem toDoItem);

    public static partial IQueryable<ToDoItemDto> ToToDoItemsDto(this IQueryable<ToDoItem> toDoItem);
}