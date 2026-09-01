using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Poc.Auth.Blazor.Extensions;
using Poc.Auth.Blazor.Components;

namespace Poc.Auth.Blazor;

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

        builder.Services.AddEntraBasicAuthentications(builder.Configuration.GetSection("Authentication:EntraIdBasic"));
        //builder.Services.AddGoogleBasicAuthentications(builder.Configuration.GetSection("Authentication:Google"));

        builder.Services.AddGraphDownstreamApi(builder.Configuration.GetSection("DownstreamApis:MicrosoftGraph"));

        // Add Authorization
        builder.Services.AddAuthorization(options =>
        {
            // By default, all incoming requests will be authorized according to the default policy
            //options.FallbackPolicy = options.DefaultPolicy;
        });

        // Add the incremental consent and conditional access handler for Blazor server side pages.  (not needed?)
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
        // When the application was deployed to Azure, the Authentication was failing, because the redirect_uri was HTTP.  In Azure AD App Registration I configured it with HTTPS (HTTP is allowed only when using localhost). The application was running inside a Linux Container in an Azure Web App.
        //app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto });

        //app.UseRouting();  (not needed?)

        // for google auth (not needed?)
        //app.UseCookiePolicy()
        //   .UseAuthentication();

        // 1️⃣ Authenticate the user (sets HttpContext.User)  (not needed?)
        //app.UseAuthentication();
        // 2️⃣ Check if the user is allowed to access the resource  (not needed?)
        //app.UseAuthorization();

        // 3️⃣ Validate CSRF tokens (AFTER authentication/authorization)
        app.UseAntiforgery();

        // Add the incremental consent and conditional access handler for Blazor server side pages. (not needed?)
        //app.MapControllers();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Services.InitializeServices();

        app.MapAuthenticationEntraEndpoints();
        //app.MapAuthenticationGoogleEndpoints();
        app.MapTestEndpoints();

        app.Run();
    }
}
