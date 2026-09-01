using System.Threading;
using System.Threading.Tasks;
using Poc.Ef.Domain.Entities;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Ef.Application.Queries;

public sealed record GetToDoQuery(int id) : IQuery<TodoItem>;

public sealed class GetToDoQueryHandler : IQueryHandler<GetToDoQuery, TodoItem>
{
	// private IDbContext _dbContext;
	private readonly IApplicationDbContextFactory _dbContextFactory;

	public GetToDoQueryHandler(
		// IDbContext dbContext, 
		IApplicationDbContextFactory dbContextFactory)
	{
		// _dbContext = dbContext;
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Result<TodoItem>> Handle(GetToDoQuery command, CancellationToken cancellationToken)
	{
		await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);
		
		var result = await _dbContext
			.GetBydIdAsync<TodoItem>(command.id);

		//TodoItem? todoItem = await _dbContext
		//	.Set<TodoItem>()
		//	.SingleOrDefaultAsync(
		//		t => t.Id == command.id,
		//		cancellationToken);
		
		return result is null ? 
			Result.Failure<TodoItem>(new Error("", "")) : 
			Result.Success(result);
	}
}