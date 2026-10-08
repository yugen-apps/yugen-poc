using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Common.Data.Commands;
using Poc.Common.Data.Extensions;
using Poc.Common.Data.Helpers;
using Poc.Common.Spectre.Commands;
using Poc.Common.Spectre.Extensions;
using Poc.Common.Spectre.Helpers;
using Poc.Ef.Application.Extensions;
using Poc.Ef.Domain.Entities;
using Poc.Ef.Infrastructure.Extensions;
using Poc.Ef.Persistence.Commands;
using Poc.Ef.Persistence.Contexts;
using Poc.Ef.WorkerService.Commands;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poc.Ef.WorkerService;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = Host.CreateApplicationBuilder(args);

		// Add Services
		builder.Services.AddApplication(builder.Configuration);
		builder.Services.AddInfrastructure(builder.Configuration);
		builder.Services.AddPersistence<ApplicationDbContext>(builder.Configuration, ServiceLifetime.Singleton);

		// Add Commands
		builder.Services.AddScoped<ICommandHandler<CompleteTodoCommand>, CompleteTodoCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<CanConnectCommand>, CanConnectCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<AddToDoCommand, TodoItem>, AddToDoCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<GetToDoCommand, TodoItem>, GetToDoCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<GetAllToDoCommand, List<TodoItem>>, GetAllToDoCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<UpdateToDoCommand, TodoItem>, UpdateToDoCommandHandler>();
		builder.Services.AddScoped<ICommandHandler<DeleteToDoCommand>, DeleteToDoCommandHandler>();

		// Add commands
		builder.Services.AddCommand<DefaultCommand>("Menu", cmd => { cmd.WithDescription("Default command that show the menu"); });
		// builder.Services.AddCommand<UpdateCommand>("Update");
		builder.Services.AddCommand<AddToDo>(nameof(AddToDo));
		builder.Services.AddCommand<GetToDo>(nameof(GetToDo));
		builder.Services.AddCommand<GetAll>(nameof(GetAll));
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
