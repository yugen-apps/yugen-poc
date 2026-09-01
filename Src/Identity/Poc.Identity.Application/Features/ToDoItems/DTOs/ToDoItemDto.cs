using System;

namespace Poc.Identity.Application.Features.ToDoItems.DTOs;

public class ToDoItemDto
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CompletedAt { get; set; }

    public Guid? TenantId { get; set; }
}