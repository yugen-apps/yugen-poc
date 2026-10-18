using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.Ef.WorkerService.Commands;
using Yugen.Common.Spectre.Commands;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Ef.WorkerService.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Spectre Commands
        services.AddCommand<DefaultCommand>("Menu", cmd => { cmd.WithDescription("Default command that show the menu"); });
        // builder.Services.AddCommand<UpdateCommand>("Update");
        services.AddCommand<AddToDo>(nameof(AddToDo));
        services.AddCommand<GetToDo>(nameof(GetToDo));
        services.AddCommand<GetAllToDo>(nameof(GetAllToDo));
        services.AddCommand<UpdateToDo>(nameof(UpdateToDo));
        services.AddCommand<DeleteToDo>(nameof(DeleteToDo));
        services.AddCommand<CompleteTodo>(nameof(CompleteTodo));
        services.AddCommand<CanConnect>(nameof(CanConnect));
        services.AddCommand<ExitCommand>("Exit", cmd => { cmd.WithDescription("A command that exit the app"); });
    }
}
