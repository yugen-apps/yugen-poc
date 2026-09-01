using System.Threading;
using System.Threading.Tasks;
using Poc.Ef.Domain.Entities;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Ef.Application.Commands;

public sealed record UpdateToDoCommand(int id, string title) : ICommand<TodoItem>;

public sealed class UpdateToDoCommandHandler : ICommandHandler<UpdateToDoCommand, TodoItem>
{
	// private IDbContext _dbContext;
	private readonly IApplicationDbContextFactory _dbContextFactory;

	public UpdateToDoCommandHandler(
		// IDbContext dbContext, 
		IApplicationDbContextFactory dbContextFactory)
	{
		// _dbContext = dbContext;
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Result<TodoItem>> Handle(UpdateToDoCommand command, CancellationToken cancellationToken)
	{
		await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);
		
		var todoItem = await _dbContext
			.GetBydIdAsync<TodoItem>(command.id);

		if (todoItem is null)
		{
			return Result.Failure<TodoItem>(new Error("", ""));
		}

		todoItem.Title = command.title;

		var result = await _dbContext.SaveChangesAsync(cancellationToken);

		return result == 0 ? 
			Result.Failure<TodoItem>(new Error("", "")) : 
			Result.Success(todoItem);
	}
}