using System.Threading;
using System.Threading.Tasks;

namespace Yugen.Common.Data;

public interface IApplicationDbContextFactory
{
    ValueTask<IDbContext> CreateAsync(CancellationToken ct = default);
}