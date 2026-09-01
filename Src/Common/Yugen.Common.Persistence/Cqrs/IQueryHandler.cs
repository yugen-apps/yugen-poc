using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Core.Models;

namespace Yugen.Common.Persistence.Cqrs;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}