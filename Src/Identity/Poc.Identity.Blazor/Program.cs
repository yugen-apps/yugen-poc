using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Poc.Identity.Blazor.Components;
using Poc.Identity.Data;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account;
using Yugen.Common.Blazor.Account.Domain;
using Yugen.Common.Blazor.Account.Models;
using Yugen.Common.Data.Extensions;

namespace Poc.Identity.Blazor;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add MudBlazor services
		builder.Services.AddMudServices();

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents();

		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddScoped<IdentityRedirectManager>();
		builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

		builder.Services.AddAuthentication(options =>
			{
				options.DefaultScheme = IdentityConstants.ApplicationScheme;
				options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
			})
			.AddGoogle(options =>
			{
				options.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId") ?? string.Empty;
				options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret") ?? string.Empty;
				options.ClaimActions.MapJsonKey(GoogleClaimTypes.Picture, "picture");
			})
			.AddIdentityCookies();

		builder.Services.AddDatabaseDeveloperPageExceptionFilter();
		builder.Services.AddPersistence<ApplicationDbContext>(builder.Configuration);

		builder.Services.AddIdentityCore<ApplicationUser>(options =>
		{
			options.SignIn.RequireConfirmedAccount = true;
			options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
		})
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddSignInManager()
		.AddDefaultTokenProviders();

		builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseMigrationsEndPoint();
		}
		else
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
			.AddInteractiveServerRenderMode()
			.AddAdditionalAssemblies([
				typeof(Yugen.Common.Blazor._Imports).Assembly,
				typeof(Yugen.Common.Blazor.Account._Imports).Assembly
			]);

		// Add additional endpoints required by the Identity /Account Razor components.
		app.MapAdditionalIdentityEndpoints();

		await app.Services.MigrateAsync<ApplicationDbContext>();

		app.Run();
	}
}
