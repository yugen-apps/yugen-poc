using Microsoft.AspNetCore.Authorization;
using Poc.Identity.Domain.Shared.Constants;
using System.Security.Claims;

namespace Poc.Identity.Blazor.Extensions;

public static class Policy
{
    public static void AddPolicy(this AuthorizationOptions options)
    {
        // By default, all incoming requests will be authorized according to the default policy
        //options.FallbackPolicy = options.DefaultPolicy;

        options.AddPolicy(PolicyConstants.SuperAdmin, policy => policy
            .RequireClaim(ClaimTypes.Role, RoleConstants.SuperAdmin));

        options.AddPolicy(PolicyConstants.Admin, policy => policy
            .RequireClaim(ClaimTypes.Role, RoleConstants.SuperAdmin, RoleConstants.Admin));

        options.AddPolicy(PolicyConstants.User, policy => policy
            .RequireClaim(ClaimTypes.Role, RoleConstants.SuperAdmin, RoleConstants.Admin, RoleConstants.User));
    }

    //public static AuthorizationPolicy DefaultPolicy =>
    //    new AuthorizationPolicyBuilder()
    //        .RequireAuthenticatedUser()
    //        .Build();

    //public static AuthorizationPolicy PmPolicy()
    //{
    //    return new AuthorizationPolicyBuilder()
    //                .RequireAssertion(context =>
    //                    context.User.HasClaim(c =>
    //                        c.Type == DevClaimType ||
    //                        c.Type == DevOpClaimType ||
    //                        c.Type == PmClaimType
    //                    )
    //                )
    //                .Build();
    //}
}