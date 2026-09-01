using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Poc.Ef.Domain.Entities;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Ef.Application.Queries;

public sealed record GetAllToDoQuery() : IQuery<List<TodoItem>>;

public sealed class GetAllToDoQueryHandler : IQueryHandler<GetAllToDoQuery, List<TodoItem>>
{
	// private IDbContext _dbContext;
	private readonly IApplicationDbContextFactory _dbContextFactory;

	public GetAllToDoQueryHandler(
		// IDbContext dbContext, 
		IApplicationDbContextFactory dbContextFactory)
	{
		// _dbContext = dbContext;
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Result<List<TodoItem>>> Handle(GetAllToDoQuery query, CancellationToken cancellationToken)
	{
		await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);
		
		var todoItems = await _dbContext
			.Set<TodoItem>()
			.ToListAsync(cancellationToken);

		return Result.Success(todoItems);
	}
}