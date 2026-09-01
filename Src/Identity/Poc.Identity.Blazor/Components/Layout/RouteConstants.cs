namespace Poc.Identity.Blazor.Components.Layout;

public static class RouteConstants
{
    public const string Home = "/";
    public const string Error = "/Error";
    public const string Info = "/Info";
    public const string NotFound = "/not-found";
    public const string Tenants = "/Tenants";

    public static class ToDo
    {
        public const string Base = "ToDo";
        public const string All = $"{Base}/All";
        public const string Tenants = $"{Base}/Tenants";
    }
}
