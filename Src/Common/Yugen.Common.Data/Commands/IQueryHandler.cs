using System.Threading;
using System.Threading.Tasks;

namespace Yugen.Common.Data.Commands;

public interface IQueryHandler<in TQuery, TResponse>
	where TQuery : IQuery<TResponse>
{
	Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}