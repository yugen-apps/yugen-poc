using Azure.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Poc.Auth.Blazor.Options;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Components.Pages.Old;

public partial class Downstream
{
    private EntraIdOptions? _entraId;
    private MicrosoftGraphOptions? _microsoftGraph;

    [Inject]
    private IConfiguration? Configuration { get; set; }

    [Inject]
    private GraphServiceClient? GraphServiceClient { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _entraId = Configuration.GetSection("EntraId").Get<EntraIdOptions>();
        _microsoftGraph = Configuration.GetSection("DownstreamApis:MicrosoftGraph").Get<MicrosoftGraphOptions>();

        await OnGetMicrosoftGraphAsync();
    }

    // https://devblogs.microsoft.com/identity/access-cloud-resources-across-tenants-without-secrets-ga/
    // https://devblogs.microsoft.com/identity/access-cloud-resources-across-tenants-without-secrets/
    // https://www.github.com/AzureAD/microsoft-identity-web/wiki/Federated-Identity-Credential-(FIC)-with-a-Managed-Service-Identity-(MSI)
    // https://learn.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme
    // https://learn.microsoft.com/en-us/graph/sdks/choose-authentication-providers
    private async Task OnGetMicrosoftGraphAsync()
    {
        // if (Env.IsDevelopment())

        // OK
        GraphServiceClient = new GraphServiceClient(new AzureCliCredential(), _microsoftGraph.UserScopes);
        //await OnGetMicrosoftGraphAsync(nameof(AzureCliCredential));

        // OK

        //await OnGetMicrosoftGraphAsync($"{nameof(GraphServiceClient)}-delegated");

        // https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-config-app-trust-managed-identity?tabs=microsoft-entra-admin-center
        var clientAssertionCredential = new ClientAssertionCredential(
           _entraId.TenantId,
           _entraId.ClientId,
           async (token) =>
           {
               // fetch Managed Identity token for the specified audience
               var tokenRequestContext = new Azure.Core.TokenRequestContext([_entraId.ClientCredentials[0].TokenExchangeUrl]);
               var accessToken = await new ManagedIdentityCredential(_entraId.ClientCredentials[0].ManagedIdentityClientId)
                                       .GetTokenAsync(tokenRequestContext)
                                       .ConfigureAwait(false);
               return accessToken.Token;
           });
        GraphServiceClient = new GraphServiceClient(clientAssertionCredential, _microsoftGraph.UserScopes);
        //await OnGetMicrosoftGraphAsync($"{nameof(ClientAssertionCredential)}-application");

        //GraphServiceClient = new GraphServiceClient(new ClientSecretCredential(_entraId.TenantId, _entraId.ClientId, _entraId.ClientCredentials[0].ClientSecret), _microsoftGraph.Scopes);
        //await OnGetAsync(nameof(ClientSecretCredential));

        // An error occurred while processing your request.

        //GraphServiceClient = new GraphServiceClient(new ManagedIdentityCredential(), _microsoftGraph.Scopes);
        //GraphServiceClient = new GraphServiceClient(new ManagedIdentityCredential(_entraId.ClientId), _microsoftGraph.Scopes);
        //GraphServiceClient = new GraphServiceClient(new ManagedIdentityCredential(_entraId.ClientCredentials[0].ManagedIdentityClientId), _microsoftGraph.Scopes);
        //await OnGetAsync($"{nameof(ManagedIdentityCredential)}-systemAssignedManagedIdentityClientId");

        //GraphServiceClient = new GraphServiceClient(new WorkloadIdentityCredential(new WorkloadIdentityCredentialOptions
        //{
        //    TenantId = _entraId.TenantId,
        //    ClientId = _entraId.ClientId,
        //    TokenFilePath = _entraId.ClientCredentials[0].TokenExchangeUrl
        //}), _microsoftGraph.Scopes);
        //await OnGetAsync(nameof(WorkloadIdentityCredential));

        // https://learn.microsoft.com/en-us/entra/msal/dotnet/acquiring-tokens/web-apps-apis/workload-identity-federation
        //var confidentialClientApplication = ConfidentialClientApplicationBuilder
        //            .Create(_entraId.ClientId)
        //            .WithClientAssertion(async (AssertionRequestOptions options) =>
        //                await new ManagedIdentityClientAssertion(_entraId.ClientCredentials[0].ManagedIdentityClientId).GetSignedAssertionAsync(default))
        //            .WithCacheOptions(CacheOptions.EnableSharedCacheOptions)
        //            .Build();
        //GraphServiceClient = new GraphServiceClient("https://graph.microsoft.com/V1.0/",
        //                        new DelegateAuthenticationProvider(async (requestMessage) =>
        //                        {
        //                            // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
        //                            AuthenticationResult result = await confidentialClientApplication.AcquireTokenForClient(_microsoftGraph.Scopes)
        //                                .ExecuteAsync();

        //                            // Add the access token in the Authorization header of the API request.
        //                            requestMessage.Headers.Authorization =
        //                                new AuthenticationHeaderValue("Bearer", result.AccessToken);
        //                        }));
        //await OnGetAsync(nameof(ManagedIdentityClientAssertion));
    }
}
