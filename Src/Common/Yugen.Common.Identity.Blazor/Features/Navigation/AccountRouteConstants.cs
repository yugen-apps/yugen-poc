namespace Yugen.Common.Identity.Blazor.Features.Navigation;

public static class AccountRouteConstants
{
    public const string Base = "Account";
    public const string AccessDenied = $"{Base}/AccessDenied";
    public const string ConfirmEmail = $"{Base}/ConfirmEmail";
    public const string ConfirmEmailChange = $"{Base}/ConfirmEmailChange";
    public const string ExternalLogin = $"{Base}/ExternalLogin";
    public const string ForgotPassword = $"{Base}/ForgotPassword";
    public const string ForgotPasswordConfirmation = $"{Base}/ForgotPasswordConfirmation";
    public const string InvalidPasswordReset = $"{Base}/InvalidPasswordReset";
    public const string InvalidUser = $"{Base}/InvalidUser";
    public const string Lockout = $"{Base}/Lockout";
    public const string Login = $"{Base}/Login";
    public const string LoginWith2fa = $"{Base}/LoginWith2fa";
    public const string LoginWithRecoveryCode = $"{Base}/LoginWithRecoveryCode";
    public const string Logout = $"{Base}/Logout";
    public const string PerformExternalLogin = $"{Base}/PerformExternalLogin";
    public const string Register = $"{Base}/Register";
    public const string RegisterConfirmation = $"{Base}/RegisterConfirmation";
    public const string ResendEmailConfirmation = $"{Base}/ResendEmailConfirmation";
    public const string ResetPassword = $"{Base}/ResetPassword";
    public const string ResetPasswordConfirmation = $"{Base}/ResetPasswordConfirmation";

    public static class Manage
    {
        public const string Base = $"{AccountRouteConstants.Base}/Manage";
        public const string ChangePassword = $"{Base}/ChangePassword";
        public const string DeletePersonalData = $"{Base}/DeletePersonalData";
        public const string Disable2fa = $"{Base}/Disable2fa";
        public const string DownloadPersonalData = $"{Base}/DownloadPersonalData";
        public const string Email = $"{Base}/Email";
        public const string EnableAuthenticator = $"{Base}/EnableAuthenticator";
        public const string ExternalLogins = $"{Base}/ExternalLogins";
        public const string GenerateRecoveryCodes = $"{Base}/GenerateRecoveryCodes";
        public const string LinkExternalLogin = $"{Base}/LinkExternalLogin";
        public const string Passkeys = $"{Base}/Passkeys";
        public const string PersonalData = $"{Base}/PersonalData";
        public const string RenamePasskey = $"{Base}/RenamePasskey";
        public const string RenamePasskeyId = $"{Base}/RenamePasskey/{{Id}}";
        public const string ResetAuthenticator = $"{Base}/ResetAuthenticator";
        public const string SetPassword = $"{Base}/SetPassword";
        public const string TwoFactorAuthentication = $"{Base}/TwoFactorAuthentication";
    }
}
