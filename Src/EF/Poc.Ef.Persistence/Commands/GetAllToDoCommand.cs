using Microsoft.EntityFrameworkCore;
using Poc.Common;
using Poc.Common.Data;
using Poc.Common.Data.Commands;
using Poc.Ef.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Commands;

public sealed record GetAllToDoCommand() : ICommand<List<TodoItem>>;

public sealed class GetAllToDoCommandHandler : ICommandHandler<GetAllToDoCommand, List<TodoItem>>
{
	private readonly IDbContext _context;

	public GetAllToDoCommandHandler(
		IDbContext context)
	{
		_context = context;
	}

	public async Task<Result<List<TodoItem>>> Handle(GetAllToDoCommand command, CancellationToken cancellationToken)
	{
		var todoItems = await _context
			.Set<TodoItem>()
			.ToListAsync(cancellationToken);

		if (todoItems is null)
		{
			return Result.Failure<List<TodoItem>>(new Error("", ""));
		}

		return Result.Success(todoItems);
	}
}