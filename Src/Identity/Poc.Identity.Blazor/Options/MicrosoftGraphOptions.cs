namespace Poc.Identity.Blazor.Options;

public class MicrosoftGraphOptions
{
    public string? BaseUrl { get; set; }

    public bool RequestAppToken { get; set; }

    public string[]? Scopes { get; set; }
}