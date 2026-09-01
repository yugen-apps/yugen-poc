using Poc.Docker.Aspnet.Components;
using Poc.Docker.Aspnet.Data;
using Poc.Docker.Aspnet.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;

namespace Poc.Docker.Aspnet;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("MsSql");
        builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString));

        // Add MudBlazor services
        builder.Services.AddMudServices();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // builder.Services.AddHealthChecks();

        // Uncomment if using System.Text.Json source generation
        // builder.Services.ConfigureHttpJsonOptions(options =>
        // {
        //     options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
        // });

        var app = builder.Build();

        // app.MapHealthChecks("/healthz");

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // CancellationTokenSource cancellation = new();
        // app.Lifetime.ApplicationStopping.Register(() =>
        // {
        //     cancellation.Cancel();
        // });

        app.MapGet("/Environment", () =>
        {
            return new EnvironmentInfo();
        });

        // This API demonstrates how to use task cancellation
        // to support graceful container shutdown via SIGTERM.
        // The method itself is an example and not useful.
        //app.MapGet("/Delay/{value}", async (int value) =>
        //{
        //    try
        //    {
        //        await Task.Delay(value, cancellation.Token);
        //    }
        //    catch(TaskCanceledException)
        //    {
        //    }
        //    return new Operation(value);
        //});

        app.Run();
    }
}
