using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Poc.Auth.Blazor.Options;

namespace Poc.Auth.Blazor.Services.Graph;

public class AppGraphService : BaseGraphService
{
    public AppGraphService(
        IOptions<EntraIdOptions> entraIdOptions,
        IOptions<MicrosoftGraphOptions> microsoftGraphOptionsOptions,
        ILogger<AppGraphService> logger) : base(
            entraIdOptions,
            microsoftGraphOptionsOptions,
            logger)
    {
        // Use the default scope, which will request the scopes configured on the app registration
        // _scopes = ["https://graph.microsoft.com/.default"];
    }

    public override GraphServiceClient GetClient()
    {
        return GraphServiceClient ??= new GraphServiceClient(GetAzureTokenCredential(), Scopes);
    }

    public override TokenCredential GetAzureTokenCredential()
    {
        return AzureTokenCredential ??= new ChainedTokenCredential(
                new ClientSecretCredential(EntraIdOptions.TenantId, EntraIdOptions.ClientId, EntraIdOptions.ClientSecret),
                new ManagedIdentityCredential("clientId")
            );
    }

}
