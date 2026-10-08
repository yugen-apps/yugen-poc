using Poc.Common;
using Poc.Common.Data;
using Poc.Common.Data.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.Persistence.Commands;

public sealed record CanConnectCommand() : ICommand;

public sealed class CanConnectCommandHandler : ICommandHandler<CanConnectCommand>
{
	private readonly IDbContext _context;

	public CanConnectCommandHandler(
		IDbContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(CanConnectCommand command, CancellationToken cancellationToken)
	{
		var canConnect = await _context.CanConnectAsync();

		var result = canConnect == true ?
			Result.Success() :
			Result.Failure(new Error(string.Empty, string.Empty));

		return result;
	}
}