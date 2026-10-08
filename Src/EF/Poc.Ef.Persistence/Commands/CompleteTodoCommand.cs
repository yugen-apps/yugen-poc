using Poc.Common;
using Poc.Common.Data;
using Poc.Common.Data.Commands;
using Poc.Ef.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Commands;

public sealed record CompleteTodoCommand(int TodoItemId) : ICommand;

public sealed class CompleteTodoCommandHandler : ICommandHandler<CompleteTodoCommand>
{
	private readonly IDbContext _context;
	//private readonly IDateTimeProvider dateTimeProvider;

	public CompleteTodoCommandHandler(
		IDbContext context)
	//IDateTimeProvider dateTimeProvider
	{
		_context = context;
		//this.dateTimeProvider = dateTimeProvider;
	}

	public async Task<Result> Handle(CompleteTodoCommand command, CancellationToken cancellationToken)
	{
		TodoItem? todoItem = await _context
			.GetBydIdAsync<TodoItem>(command.TodoItemId);

		if (todoItem is null)
		{
			return Result.Failure(new Error("", ""));
		}

		todoItem.IsCompleted = true;
		todoItem.CompletedAt = DateTime.UtcNow;
		//todoItem.CompletedAt = dateTimeProvider.UtcNow;

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}