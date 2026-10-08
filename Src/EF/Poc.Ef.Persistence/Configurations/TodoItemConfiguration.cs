using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poc.Ef.Domain.Entities;

namespace Poc.Ef.Persistence.Configurations;

/// <summary>
/// Contains the <see cref="Race"/> entity configuration.
/// </summary>
internal sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
	/// <inheritdoc />
	public void Configure(EntityTypeBuilder<TodoItem> builder)
	{
		builder.HasKey(race => race.Id);
	}
}