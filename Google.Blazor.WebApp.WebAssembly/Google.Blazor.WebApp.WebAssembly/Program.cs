using Google.Blazor.WebApp.WebAssembly.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Google.Blazor.WebApp.WebAssembly
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie();

            builder.Services.AddAuthentication()
                            .AddGoogle(options =>
                            {
                                options.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId");
                                options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret");
                                //options.ClaimActions.MapJsonKey("urn:google:profile", "link");
                                //options.ClaimActions.MapJsonKey("urn:google:image", "picture");
                            });

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAntiforgery();

            app.UseCookiePolicy()
               .UseAuthentication();

            // Add the incremental consent and conditional access handler for Blazor server side pages.
            app.MapControllers();

            app.MapRazorComponents<App>()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
