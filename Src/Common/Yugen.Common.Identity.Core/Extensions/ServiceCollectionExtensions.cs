using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yugen.Common.Identity.Domain.Entities;

namespace Yugen.Common.Identity.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityCore<T>(
        this IServiceCollection services,
        IConfiguration configuration) where T : DbContext
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        });

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
        })
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<T>()
        .AddSignInManager()
        .AddDefaultTokenProviders();
        return services;
    }
}