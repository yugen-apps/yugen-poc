using System.Collections.Generic;
using MudBlazor;
using Yugen.Common.Blazor.Navigation;

namespace Yugen.Common.Identity.Blazor.Features.Navigation;

public static class AccountMenuConstants
{
    public static List<MenuItem> Items =>
    [
        Manage.Base,
        //Manage.Profile,
        //Manage.Email,
        //Manage.ChangePassword,
        //Manage.TwoFactorAuthenticationPath,
        //Manage.PasskeysPath,
        //Manage.PersonalData
    ];

    public static MenuItem Login => new()
    {
        Href = AccountRouteConstants.Login,
        Icon = Icons.Material.Filled.Login,
        Title = "Log In"
    };

    public static MenuItem Register => new()
    {
        Href = AccountRouteConstants.Register,
        Icon = Icons.Material.Filled.Person,
        IsDisabled = true,
        Title = "Register"
    };

    public static class Manage
    {
        public static MenuItem Base => new()
        {
            Icon = Icons.Material.Filled.Person,
            Title = "Manage",
            MenuSubItems = [
                Profile,
                Email,
                ChangePassword,
                //ExternalLogins,
                TwoFactorAuthenticationPath,
                PasskeysPath,
                PersonalData
            ]
        };

        public static MenuItem Profile => new()
        {
            Href = AccountRouteConstants.Manage.Base,
            Icon = Icons.Material.Filled.Person,
            Title = "Profile"
        };

        public static MenuItem Email => new()
        {
            Href = AccountRouteConstants.Manage.Email,
            Icon = Icons.Material.Filled.Email,
            Title = "Email"
        };

        public static MenuItem ChangePassword => new()
        {
            Href = AccountRouteConstants.Manage.ChangePassword,
            Icon = Icons.Material.Filled.Lock,
            Title = "Password"
        };

        public static MenuItem ExternalLogins => new()
        {
            Href = AccountRouteConstants.Manage.ExternalLogins,
            Icon = Icons.Material.Filled.PhoneLocked,
            Title = "External logins"
        };

        public static MenuItem TwoFactorAuthenticationPath => new()
        {
            Href = AccountRouteConstants.Manage.TwoFactorAuthentication,
            Icon = Icons.Material.Filled.LockClock,
            Title = "Two-factor authentication"
        };

        public static MenuItem PasskeysPath => new()
        {
            Href = AccountRouteConstants.Manage.Passkeys,
            Icon = Icons.Material.Filled.Key,
            Title = "Passkeys"
        };

        public static MenuItem PersonalData => new()
        {
            Href = AccountRouteConstants.Manage.PersonalData,
            Icon = Icons.Material.Filled.PersonRemove,
            Title = "Personal data"
        };
    }
}