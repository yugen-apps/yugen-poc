using System;
using Yugen.Common.Identity.Domain.Entities;
using Yugen.Common.Persistence.Entities;

namespace Poc.Identity.Domain.Entities;

public class ToDoItem : BaseEntity
{
    public required string Title { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CompletedAt { get; set; }

    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}