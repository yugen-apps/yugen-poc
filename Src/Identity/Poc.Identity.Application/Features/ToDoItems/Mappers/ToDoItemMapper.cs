using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Poc.Ef.Application.Mappers;

[Mapper]
public static partial class ToDoItemMapper
{
    [MapperIgnoreSource(nameof(ToDoItem.Tenant))]
    public static partial ToDoItemDto ToToDoItemDto(this ToDoItem toDoItem);
}