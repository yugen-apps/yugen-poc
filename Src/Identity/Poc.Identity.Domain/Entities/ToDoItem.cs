using System;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Data.Entities;

namespace Poc.Identity.Domain.Entities;

public class ToDoItem : BaseEntity
{
    public required string Title { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CompletedAt { get; set; }

    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}