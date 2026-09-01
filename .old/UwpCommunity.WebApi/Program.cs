using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication;
using UwpCommunity.WebApi.Auth;
using UwpCommunity.WebApi.Services;

namespace UwpCommunity.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);          

            builder.Services.AddControllers(); 

            builder.Services.AddSingleton<DiscordHttpClientService>();

            builder.Services.AddAuthentication("DiscordAuthentication")
                .AddScheme<AuthenticationSchemeOptions, DiscordAuthenticationHandler>("DiscordAuthentication", null);
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
