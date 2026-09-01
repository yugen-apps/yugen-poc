using System;
using System.Threading;
using System.Threading.Tasks;
using Poc.Ef.Domain.Entities;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Ef.Application.Commands;

public sealed record CompleteTodoCommand(int TodoItemId) : ICommand;

public sealed class CompleteTodoCommandHandler : ICommandHandler<CompleteTodoCommand>
{
	// private IDbContext _dbContext;
	private readonly IApplicationDbContextFactory _dbContextFactory;
	//private readonly IDateTimeProvider dateTimeProvider;

	public CompleteTodoCommandHandler(
		// IDbContext dbContext, 
		IApplicationDbContextFactory dbContextFactory)
	{
		// _dbContext = dbContext;
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Result> Handle(CompleteTodoCommand command, CancellationToken cancellationToken)
	{
		await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);
		
		var todoItem = await _dbContext
			.GetBydIdAsync<TodoItem>(command.TodoItemId);

		if (todoItem is null)
		{
			return Result.Failure(new Error("", ""));
		}

		todoItem.IsCompleted = true;
		todoItem.CompletedAt = DateTime.UtcNow;
		//todoItem.CompletedAt = dateTimeProvider.UtcNow;

		var result = await _dbContext.SaveChangesAsync(cancellationToken);
		
		return result == 0 ? 
			Result.Failure(new Error("", "")) : 
			Result.Success();
	}
}