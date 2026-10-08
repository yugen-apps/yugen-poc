using Poc.Common;
using Poc.Common.Data;
using Poc.Common.Data.Commands;
using Poc.Ef.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Commands;

public sealed record UpdateToDoCommand(int id, string title) : ICommand<TodoItem>;

public sealed class UpdateToDoCommandHandler : ICommandHandler<UpdateToDoCommand, TodoItem>
{
	private readonly IDbContext _context;

	public UpdateToDoCommandHandler(
		IDbContext context)
	{
		_context = context;
	}

	public async Task<Result<TodoItem>> Handle(UpdateToDoCommand command, CancellationToken cancellationToken)
	{
		TodoItem? todoItem = await _context
			.GetBydIdAsync<TodoItem>(command.id);

		if (todoItem is null)
		{
			return Result.Failure<TodoItem>(new Error("", ""));
		}

		todoItem.Title = command.title;

		var result = await _context.SaveChangesAsync(cancellationToken);

		if (result == 0)
		{
			return Result.Failure<TodoItem>(new Error("", ""));
		}

		return Result.Success(todoItem);
	}
}