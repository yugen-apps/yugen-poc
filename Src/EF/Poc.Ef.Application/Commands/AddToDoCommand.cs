using System.Threading;
using System.Threading.Tasks;
using Poc.Ef.Domain.Entities;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Ef.Application.Commands;

public sealed record AddToDoCommand(string title) : ICommand<TodoItem>;

public sealed class AddToDoCommandHandler : ICommandHandler<AddToDoCommand, TodoItem>
{
	// private IDbContext _dbContext;
	private readonly IApplicationDbContextFactory _dbContextFactory;

	public AddToDoCommandHandler(
		// IDbContext dbContext, 
		IApplicationDbContextFactory dbContextFactory)
	{
		// _dbContext = dbContext;
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Result<TodoItem>> Handle(AddToDoCommand command, CancellationToken cancellationToken)
	{
		await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);
		
		var todoItem = new TodoItem { Title = command.title };
		
		var todoItemEntityEntry = await _dbContext
			.AddAsync(todoItem);

		var result = await _dbContext.SaveChangesAsync(cancellationToken);
		
		return result == 0 ? 
			Result.Failure<TodoItem>(new Error("", "")) : 
			Result.Success(todoItemEntityEntry.Entity);
	}
}