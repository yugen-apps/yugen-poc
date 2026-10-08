using Poc.Common;
using Poc.Common.Data;
using Poc.Common.Data.Commands;
using Poc.Ef.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Commands;

public sealed record DeleteToDoCommand(int id) : ICommand;

public sealed class DeleteToDoCommandHandler : ICommandHandler<DeleteToDoCommand>
{
	private readonly IDbContext _context;

	public DeleteToDoCommandHandler(
		IDbContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(DeleteToDoCommand command, CancellationToken cancellationToken)
	{
		var result = await _context
			.DeleteAsync<TodoItem>(command.id);

		if (result == 0)
		{
			return Result.Failure<TodoItem>(new Error("", ""));
		}

		return Result.Success();
	}
}