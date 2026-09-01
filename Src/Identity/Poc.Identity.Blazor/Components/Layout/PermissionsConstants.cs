using Poc.Identity.Domain.Shared.Constants;

namespace Poc.Identity.Blazor.Components.Layout;

public static class PermissionsConstants
{
    public const string Info = PolicyConstants.SuperAdmin;
    public const string Tenants = PolicyConstants.SuperAdmin;

    public static class ToDo
    {
        public const string Base = PolicyConstants.User;
        public const string All = PolicyConstants.SuperAdmin;
        public const string Tenants = PolicyConstants.User;
    }
}