using Poc.Common;
using Poc.Common.Data;
using Poc.Common.Data.Commands;
using Poc.Ef.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Commands;

public sealed record GetToDoCommand(int id) : ICommand<TodoItem>;

public sealed class GetToDoCommandHandler : ICommandHandler<GetToDoCommand, TodoItem>
{
	private readonly IDbContext _context;

	public GetToDoCommandHandler(
		IDbContext context)
	{
		_context = context;
	}

	public async Task<Result<TodoItem>> Handle(GetToDoCommand command, CancellationToken cancellationToken)
	{
		TodoItem? todoItem = await _context
			.GetBydIdAsync<TodoItem>(command.id);

		//TodoItem? todoItem = await _context
		//	.Set<TodoItem>()
		//	.SingleOrDefaultAsync(
		//		t => t.Id == command.id,
		//		cancellationToken);

		if (todoItem is null)
		{
			return Result.Failure<TodoItem>(new Error("", ""));
		}

		return Result.Success(todoItem);
	}
}