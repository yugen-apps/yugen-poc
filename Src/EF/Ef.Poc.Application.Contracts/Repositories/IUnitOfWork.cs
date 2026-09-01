using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ef.Poc.Application.Contracts.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> Save(CancellationToken cancellationToken);

    Task Rollback();
}
