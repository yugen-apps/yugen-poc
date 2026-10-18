using System.Threading;
using System.Threading.Tasks;
using Yugen.Common;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;

namespace Poc.Identity.Application.Features.Helpers.Commands;

public sealed record CanConnectCommand() : ICommand;

public sealed class CanConnectCommandHandler : ICommandHandler<CanConnectCommand>
{
    // private IDbContext _dbContext;
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public CanConnectCommandHandler(
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result> Handle(CanConnectCommand command, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateAsync(cancellationToken);

        var canConnect = await dbContext.CanConnectAsync();

        var result = canConnect ?
            Result.Success() :
            Result.Failure("Error");

        return result;
    }
}