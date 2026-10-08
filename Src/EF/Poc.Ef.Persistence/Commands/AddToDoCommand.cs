using Poc.Common;
using Poc.Common.Data;
using Poc.Common.Data.Commands;
using Poc.Ef.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Commands;

public sealed record AddToDoCommand(string title) : ICommand<TodoItem>;

public sealed class AddToDoCommandHandler : ICommandHandler<AddToDoCommand, TodoItem>
{
	private readonly IDbContext _context;

	public AddToDoCommandHandler(
		IDbContext context)
	{
		_context = context;
	}

	public async Task<Result<TodoItem>> Handle(AddToDoCommand command, CancellationToken cancellationToken)
	{
		var todoItem = await _context
			.AddAsync(new TodoItem { Title = command.title });

		if (todoItem is null)
		{
			return Result.Failure<TodoItem>(new Error("", ""));
		}

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success(todoItem.Entity);
	}
}