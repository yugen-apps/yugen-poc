namespace Poc.Identity.Blazor.Options;

public class EntraIdOptions
{
    public string? CallbackPath { get; set; }
    public string? ClientId { get; set; }
    public string? Domain { get; set; }
    public string? Instance { get; set; }
    public string? ResponseType { get; set; }
    public string? TenantId { get; set; }


    public ClientCredentialsOptions[]? ClientCredentials { get; set; }
}
