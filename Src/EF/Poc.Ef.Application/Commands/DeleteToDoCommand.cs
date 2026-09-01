using System.Threading;
using System.Threading.Tasks;
using Poc.Ef.Domain.Entities;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Ef.Application.Commands;

public sealed record DeleteToDoCommand(int id) : ICommand;

public sealed class DeleteToDoCommandHandler : ICommandHandler<DeleteToDoCommand>
{
	// private IDbContext _dbContext;
	private readonly IApplicationDbContextFactory _dbContextFactory;

	public DeleteToDoCommandHandler(
		// IDbContext dbContext, 
		IApplicationDbContextFactory dbContextFactory)
	{
		// _dbContext = dbContext;
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Result> Handle(DeleteToDoCommand command, CancellationToken cancellationToken)
	{
		await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);
		
		var result = await _dbContext
			.DeleteAsync<TodoItem>(command.id);

		return result == 0 ? 
			Result.Failure(new Error("", "")) : 
			Result.Success();
	}
}