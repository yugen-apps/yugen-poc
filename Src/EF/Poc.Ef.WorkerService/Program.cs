using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Ef.Application.Commands;
using Poc.Ef.Application.Extensions;
using Poc.Ef.Application.Queries;
using Poc.Ef.Domain.Entities;
using Poc.Ef.Infrastructure.Extensions;
using Poc.Ef.Persistence.Contexts;
using Poc.Ef.WorkerService.Commands;
using Spectre.Console.Cli;
using Yugen.Common.Data;
using Yugen.Common.Data.Commands;
using Yugen.Common.Data.Extensions;
using Yugen.Common.Data.Helpers;
using Yugen.Common.Spectre.Commands;
using Yugen.Common.Spectre.Extensions;
using Yugen.Common.Spectre.Helpers;

namespace Poc.Ef.WorkerService;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = Host.CreateApplicationBuilder(args);

		// Add Services
		builder.Services.AddApplication(builder.Configuration);
		builder.Services.AddInfrastructure(builder.Configuration);
		builder.Services.AddPersistenceFactory<ApplicationDbContext>(builder.Configuration);
		builder.Services.AddScoped<IApplicationDbContextFactory, ApplicationDbContextFactory>();

		// Add Commands
		builder.Services.AddScoped<ICommandHandler<CompleteTodoCommand>, CompleteTodoCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<CanConnectCommand>, CanConnectCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<AddToDoCommand, TodoItem>, AddToDoCommandHandler>();
		builder.Services.AddScoped<IQueryHandler<GetToDoQuery, TodoItem>, GetToDoQueryHandler>();
		builder.Services.AddScoped<IQueryHandler<GetAllToDoQuery, List<TodoItem>>, GetAllToDoQueryHandler>();
		builder.Services.AddScoped<ICommandHandler<UpdateToDoCommand, TodoItem>, UpdateToDoCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<DeleteToDoCommand>, DeleteToDoCommandHandler>();

		// Add commands
		builder.Services.AddCommand<DefaultCommand>("Menu", cmd => { cmd.WithDescription("Default command that show the menu"); });
		// builder.Services.AddCommand<UpdateCommand>("Update");
		builder.Services.AddCommand<AddToDo>(nameof(AddToDo));
		builder.Services.AddCommand<GetToDo>(nameof(GetToDo));
		builder.Services.AddCommand<GetAllToDo>(nameof(GetAllToDo));
		builder.Services.AddCommand<UpdateToDo>(nameof(UpdateToDo));
		builder.Services.AddCommand<DeleteToDo>(nameof(DeleteToDo));
		builder.Services.AddCommand<CompleteTodo>(nameof(CompleteTodo));
		builder.Services.AddCommand<CanConnect>(nameof(CanConnect));
		builder.Services.AddCommand<ExitCommand>("Exit", cmd => { cmd.WithDescription("A command that exit the app"); });

		// The standard call save for the commands will be pre-added & configured
		builder.UseSpectreConsole<DefaultCommand>(config =>
		{
			// All commands above are passed to config.AddCommand() by this point
#if DEBUG
			config.PropagateExceptions();
			config.ValidateExamples();
#endif
			config.UseBasicExceptionHandler();
		});

		var app = builder.Build();


		app.Services.EnsureCreated<ApplicationDbContext>();

		await app.RunAsync();

		SqliteInMemoryHelper.Dispose();
	}
}
