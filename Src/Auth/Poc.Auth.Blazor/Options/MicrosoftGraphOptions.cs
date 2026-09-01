namespace Poc.Auth.Blazor.Options;

public class MicrosoftGraphOptions
{
    public string? BaseUrl { get; set; }

    public bool RequestAppToken { get; set; }

    public string? Version { get; set; }

    public string[]? UserScopes { get; set; }

    public string[]? AppScopes { get; set; }
}