using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Poc.Identity.Blazor.Components;
using Poc.Identity.Blazor.Extensions;

namespace Poc.Identity.Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add MudBlazor services
        builder.Services.AddMudServices();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Add Authentication
        builder.Services.AddCascadingAuthenticationState();

        //builder.Services.AddEntraBasicAuthentications(builder.Configuration, "Authentication:EntraIdBasic");
        builder.Services.AddGoogleBasicAuthentications(builder.Configuration, "Authentication:Google");

        // Add Authorization
        builder.Services.AddAuthorization(options =>
        {
            // By default, all incoming requests will be authorized according to the default policy
            //options.FallbackPolicy = options.DefaultPolicy;
        });

        // Add the incremental consent and conditional access handler for Blazor server side pages.
        // builder.Services.AddMicrosoftIdentityConsentHandler();

        // Add Services
        builder.Services.ConfigureServices(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

        app.UseHttpsRedirection();
        // https://jaliyaudagedara.blogspot.com/2024/11/blazor-web-app-authentication-redirect.html
        //app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto });

        //app.UseRouting();

        // for google auth
        //app.UseCookiePolicy()
        //   .UseAuthentication();

        // 1️⃣ Authenticate the user (sets HttpContext.User)
        //app.UseAuthentication();
        // 2️⃣ Check if the user is allowed to access the resource
        //app.UseAuthorization();

        // 3️⃣ Validate CSRF tokens (AFTER authentication/authorization)
        app.UseAntiforgery();

        // Add the incremental consent and conditional access handler for Blazor server side pages.
        //app.MapControllers();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Services.InitializeServices();

        //app.MapAuthenticationEntraEndpoints();
        app.MapAuthenticationGoogleEndpoints();
        app.MapTestEndpoints();

        app.Run();
    }
}
