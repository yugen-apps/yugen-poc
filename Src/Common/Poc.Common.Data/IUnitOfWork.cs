using System.Threading;
using System.Threading.Tasks;

namespace Poc.Common.Data;

public interface IUnitOfWork
{
    void Dispose();
    Task Rollback();
    Task<int> Save(CancellationToken cancellationToken);
}