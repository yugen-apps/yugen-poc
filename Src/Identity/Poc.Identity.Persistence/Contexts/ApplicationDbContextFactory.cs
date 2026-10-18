using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yugen.Common.Data;

namespace Poc.Identity.Persistence.Contexts;

public sealed class ApplicationDbContextFactory : IApplicationDbContextFactory
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public ApplicationDbContextFactory(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public ValueTask<IDbContext> CreateAsync(CancellationToken ct = default)
    {
        var dbContext = _dbContextFactory.CreateDbContext();
        return new ValueTask<IDbContext>(dbContext);
    }
}