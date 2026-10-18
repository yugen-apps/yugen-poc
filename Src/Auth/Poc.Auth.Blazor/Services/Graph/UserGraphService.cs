using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Poc.Auth.Blazor.Options;

namespace Poc.Auth.Blazor.Services.Graph;

public class UserGraphService : BaseGraphService
{
    public UserGraphService(
        IOptions<EntraIdOptions> entraIdOptions,
        IOptions<MicrosoftGraphOptions> microsoftGraphOptionsOptions,
        ILogger<UserGraphService> logger) : base(
            entraIdOptions,
            microsoftGraphOptionsOptions,
            logger)
    {
        //_scopes = [
        //	"User.Read",
        //	"User.Read.All",
        //	"User.ReadBasic.All",
        //	"Group.Read.All"
        //];
    }

    public override GraphServiceClient GetClient()
    {
        return GraphServiceClient ??= new GraphServiceClient(GetAzureTokenCredential(), Scopes);
    }

    public override TokenCredential GetAzureTokenCredential()
    {
        // user
        return AzureTokenCredential ??= new ChainedTokenCredential(
            new AzureCliCredential()
        //GetDeviceCodeCredential()
        //new VisualStudioCredential()
        //new VisualStudioCodeCredential()
        //new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions()
        //{
        //    TokenCachePersistenceOptions = new TokenCachePersistenceOptions(),
        //    TenantId = _entraIdOptions.TenantId,
        //    //ClientId = _entraIdOptions.ClientId,
        //    //RedirectUri = new Uri("http://localhost:8080")
        //})
        );
    }

    //public override TokenCredential GetAzureTokenCredential()
    //{
    //	return _azureTokenCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
    //	{
    //		ExcludeAzureCliCredential = true,
    //		ExcludeAzureDeveloperCliCredential = true,
    //		ExcludeAzurePowerShellCredential = true,
    //		ExcludeInteractiveBrowserCredential = true,
    //		ExcludeVisualStudioCodeCredential = true,
    //		ExcludeVisualStudioCredential = true,

    //		ExcludeEnvironmentCredential = false,
    //		ExcludeManagedIdentityCredential = false,
    //		ExcludeSharedTokenCacheCredential = false,
    //		ExcludeWorkloadIdentityCredential = false
    //	});
    //}

    private DeviceCodeCredential GetDeviceCodeCredential()
    // Func<DeviceCodeInfo, CancellationToken, Task> deviceCodePrompt)
    {
        var options = new DeviceCodeCredentialOptions
        {
            ClientId = EntraIdOptions.ClientId,
            TenantId = EntraIdOptions.TenantId,
            //DeviceCodeCallback = deviceCodePrompt,
        };

        return new DeviceCodeCredential(options);
    }
}