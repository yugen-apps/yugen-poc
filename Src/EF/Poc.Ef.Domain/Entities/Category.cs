using Poc.Common.Data.Entities;
using System;

namespace Poc.Ef.Domain.Entities;

public class TodoItem : BaseEntity
{
	public required string Title { get; set; }

	public bool IsCompleted { get; set; }

	public DateTime CompletedAt { get; set; }
}