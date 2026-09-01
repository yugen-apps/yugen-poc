using MudBlazor;
using System.Collections.Generic;
using Yugen.Common.Blazor.Models;

namespace Yugen.Common.Blazor.Account.Navigation;

public static class AccountMenuConstants
{
    public const string AccountPath = "Account";
    public const string AccountAccessDeniedPath = $"{AccountPath}/AccessDenied";
    public const string AccountConfirmEmailPath = $"{AccountPath}/ConfirmEmail";
    public const string AccountConfirmEmailChangePath = $"{AccountPath}/ConfirmEmailChange";
    public const string AccountExternalLoginPath = $"{AccountPath}/ExternalLogin";
    public const string AccountForgotPasswordPath = $"{AccountPath}/ForgotPassword";
    public const string AccountForgotPasswordConfirmationPath = $"{AccountPath}/ForgotPasswordConfirmation";
    public const string AccountInvalidPasswordResetPath = $"{AccountPath}/InvalidPasswordReset";
    public const string AccountInvalidUserPath = $"{AccountPath}/InvalidUser";
    public const string AccountLockoutPath = $"{AccountPath}/Lockout";
    public const string AccountLoginPath = $"{AccountPath}/Login";
    public const string AccountLoginWith2faPath = $"{AccountPath}/LoginWith2fa";
    public const string AccountLoginWithRecoveryCodePath = $"{AccountPath}/LoginWithRecoveryCode";
    public const string AccountLogoutPath = $"{AccountPath}/Logout";
    public const string AccountPerformExternalLoginPath = $"{AccountPath}/PerformExternalLogin";
    public const string AccountRegisterPath = $"{AccountPath}/Register";
    public const string AccountRegisterConfirmationPath = $"{AccountPath}/RegisterConfirmation";
    public const string AccountResendEmailConfirmationPath = $"{AccountPath}/ResendEmailConfirmation";
    public const string AccountResetPasswordPath = $"{AccountPath}/ResetPassword";
    public const string AccountResetPasswordConfirmationPath = $"{AccountPath}/ResetPasswordConfirmation";

    public const string AccountManagePath = $"{AccountPath}/Manage";
    public const string AccountManageChangePasswordPath = $"{AccountManagePath}/ChangePassword";
    public const string AccountManageDeletePersonalDataPath = $"{AccountManagePath}/DeletePersonalData";
    public const string AccountManageDisable2faPath = $"{AccountManagePath}/Disable2fa";
    public const string AccountManageDownloadPersonalDataPath = $"{AccountManagePath}/DownloadPersonalData";
    public const string AccountManageEmailPath = $"{AccountManagePath}/Email";
    public const string AccountManageEnableAuthenticatorPath = $"{AccountManagePath}/EnableAuthenticator";
    public const string AccountManageExternalLoginsPath = $"{AccountManagePath}/ExternalLogins";
    public const string AccountManageGenerateRecoveryCodesPath = $"{AccountManagePath}/GenerateRecoveryCodes";
    public const string AccountManageLinkExternalLoginPath = $"{AccountManagePath}/LinkExternalLogin";
    public const string AccountManagePasskeysPath = $"{AccountManagePath}/Passkeys";
    public const string AccountManagePersonalDataPath = $"{AccountManagePath}/PersonalData";
    public const string AccountManageRenamePasskeyPath = $"{AccountManagePath}/RenamePasskey";
    public const string AccountManageRenamePasskeyRoute = $"{AccountManagePath}/RenamePasskey/{{Id}}";
    public const string AccountManageResetAuthenticatorPath = $"{AccountManagePath}/ResetAuthenticator";
    public const string AccountManageSetPasswordPath = $"{AccountManagePath}/SetPassword";
    public const string AccountManageTwoFactorAuthenticationPath = $"{AccountManagePath}/TwoFactorAuthentication";

    public static List<MenuItem> AccountManageMenuItems =>
    [
        AccountManageMenuItem,
        AccountManageEmailMenuItem,
        AccountManageChangePasswordMenuItem,
        AccountManageExternalLoginsMenuItem,
        AccountManageTwoFactorAuthenticationPathMenuItem,
        AccountManagePasskeysPathMenuItem,
        AccountManagePersonalDataMenuItem
    ];

    public static MenuItem AccountManageMenuItem => new()
    {
        Path = AccountManagePath,
        Icon = Icons.Material.Filled.Person,
        Title = "Profile"
    };

    public static MenuItem AccountManageEmailMenuItem => new()
    {
        Path = AccountManageEmailPath,
        Icon = Icons.Material.Filled.Email,
        Title = "Email"
    };

    public static MenuItem AccountManageChangePasswordMenuItem => new()
    {
        Path = AccountManageChangePasswordPath,
        Icon = Icons.Material.Filled.Lock,
        Title = "Password"
    };

    public static MenuItem AccountManageExternalLoginsMenuItem => new()
    {
        Path = AccountManageExternalLoginsPath,
        Icon = Icons.Material.Filled.PhoneLocked,
        Title = "External logins"
    };

    public static MenuItem AccountManageTwoFactorAuthenticationPathMenuItem => new()
    {
        Path = AccountManageTwoFactorAuthenticationPath,
        Icon = Icons.Material.Filled.LockClock,
        Title = "Two-factor authentication"
    };

    public static MenuItem AccountManagePasskeysPathMenuItem => new()
    {
        Path = AccountManagePasskeysPath,
        Icon = Icons.Material.Filled.Key,
        Title = "Passkeys"
    };

    public static MenuItem AccountManagePersonalDataMenuItem => new()
    {
        Path = AccountManagePersonalDataPath,
        Icon = Icons.Material.Filled.PersonRemove,
        Title = "Personal data"
    };

    public static MenuItem AccountLoginMenuItem => new()
    {
        Path = AccountLoginPath,
        Icon = Icons.Material.Filled.Login,
        Title = "Log In"
    };

    public static MenuItem AccountRegisterMenuItem => new()
    {
        Path = AccountRegisterPath,
        Icon = Icons.Material.Filled.PlusOne,
        IsDisabled = true,
        Title = "Register"
    };
}