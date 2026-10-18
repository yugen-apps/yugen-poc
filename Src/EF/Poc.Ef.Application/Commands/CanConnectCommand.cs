using System.Threading;
using System.Threading.Tasks;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Ef.Application.Commands;

public sealed record CanConnectCommand() : ICommand;

public sealed class CanConnectCommandHandler : ICommandHandler<CanConnectCommand>
{
    // private IDbContext _dbContext;
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public CanConnectCommandHandler(
        // IDbContext dbContext, 
        IApplicationDbContextFactory dbContextFactory)
    {
        // _dbContext = dbContext;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result> Handle(CanConnectCommand command, CancellationToken cancellationToken)
    {
        await using var _dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        var canConnect = await _dbContext.CanConnectAsync();

        var result = canConnect ?
            Result.Success() :
            Result.Failure("Error");

        return result;
    }
}