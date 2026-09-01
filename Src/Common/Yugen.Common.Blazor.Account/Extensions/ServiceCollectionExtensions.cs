

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yugen.Common.Blazor.Account.Features.Navigation;

namespace Yugen.Common.Blazor.Account.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountMenu(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AccountMenuConfig>(options => options.Items = AccountMenuConstants.Items);
        return services;
    }
}
