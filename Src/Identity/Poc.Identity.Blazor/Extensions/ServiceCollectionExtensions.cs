using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Identity.Blazor.Components;
using Poc.Identity.Blazor.Components.Layout;
using Yugen.Common.Core.Info;
using Yugen.Common.Identity.Blazor;
using Yugen.Common.Web.Info;

namespace Poc.Identity.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // services.AddHealthChecks();

        // Uncomment if using System.Text.Json source generation
        // services.ConfigureHttpJsonOptions(options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));

        services.Configure<MenuConfig>(options => options.Items = MenuConstants.Items);

        services.AddAuthorization(options => options.AddPolicy());

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<SystemInfoService>();
        services.AddScoped<WebHostInfoService>();
        return services;
    }

    public static WebApplication ConfigureServer(
        this WebApplication app,
        IConfiguration configuration)
    {
        // app.MapHealthChecks("/healthz");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler(RouteConstants.Error);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseStatusCodePagesWithReExecute(RouteConstants.NotFound, createScopeForStatusCodePages: true);

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies([
                typeof(Yugen.Common.Blazor._Imports).Assembly,
                typeof(Yugen.Common.Identity.Blazor._Imports).Assembly
            ]);

        // CancellationTokenSource cancellation = new();
        // app.Lifetime.ApplicationStopping.Register(() =>
        // {
        //     cancellation.Cancel();
        // });

        //app.MapGet("/Environment", () =>
        //{
        //    return new EnvironmentInfo();
        //});

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

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

        return app;
    }
}
