using Microsoft.Extensions.DependencyInjection;
using Poc.ConsoleApp.Commands;
using Poc.ConsoleApp.Commands.Grouping;
using Poc.ConsoleApp.Commands.Json;
using Poc.ConsoleApp.Commands.Lifetime;
using Yugen.Common.Spectre.Commands;
using Yugen.Common.Spectre.Extensions;

namespace Poc.ConsoleApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        // Add Spectre Commands
        services.AddCommand<DefaultCommand>("Menu", cmd => { cmd.WithDescription("Default command that show the menu"); });
        services.AddCommand<HelloWorldCommand>("HelloWorld", cmd => { cmd.WithAlias("h"); });
        services.AddCommand<GroupingCommand>("Grouping");
        services.AddCommand<LifetimeCommand>("Lifetime");
        services.AddCommand<TaskCommand>("Task");
        services.AddCommand<JsonCommand>("Json");
        services.AddCommand<ExitCommand>("Exit");
    }
}
