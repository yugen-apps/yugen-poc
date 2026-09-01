using Azure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Poc.Auth.Blazor.Options;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Services.Graph;

public abstract class BaseGraphService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    protected readonly EntraIdOptions EntraIdOptions;
    protected readonly MicrosoftGraphOptions MicrosoftGraphOptionsOptions;

    private readonly ITokenAcquisition? _tokenAcquisition = null;
    private readonly IAuthorizationHeaderProvider? _authorizationHeaderProvider = null;
    private readonly ILogger<BaseGraphService> _logger;

    protected GraphServiceClient? GraphServiceClient;
    protected TokenCredential? AzureTokenCredential;
    protected string[] Scopes = ["https://graph.microsoft.com/.default"];

    public BaseGraphService(
        IOptions<EntraIdOptions> entraIdOptions,
        IOptions<MicrosoftGraphOptions> microsoftGraphOptionsOptions,
        ILogger<BaseGraphService> logger)
    {
        EntraIdOptions = entraIdOptions.Value;
        MicrosoftGraphOptionsOptions = microsoftGraphOptionsOptions.Value;
        _logger = logger;

        Scopes = MicrosoftGraphOptionsOptions.Scopes ?? [];
    }

    public virtual GraphServiceClient GetClient() => throw new NotImplementedException();

    public virtual TokenCredential GetAzureTokenCredential() => throw new NotImplementedException();

    public async Task<string?> GetTokenFromAzureTokenCredentialAsync()
    {
        string? result;

        // Request token with given scopes
        var context = new TokenRequestContext(Scopes);
        var response = await GetAzureTokenCredential().GetTokenAsync(context, CancellationToken.None);
        result = response.Token;

        return result;
    }

    public async Task<string?> GetTokenFromAuthorizationHeaderProviderAsync()
    {
        string? result;

        try
        {
            result = await _authorizationHeaderProvider?.CreateAuthorizationHeaderForUserAsync(Scopes)!;
        }
        catch (Exception ex)
        {
            result = ex.Message;
            //ConsentHandler.HandleException(ex);
        }

        return result;
    }

    public async Task<string?> GetTokenFromTokenAcquisitionAsync()
    {
        string? result;

        try
        {
            // Getting token from Azure Active Directory
            result = await _tokenAcquisition?.GetAccessTokenForUserAsync(Scopes)!;
        }
        catch (Exception ex)
        {
            result = ex.Message;
            //ConsentHandler.HandleException(ex);
        }

        return result;
    }


    public async Task<string?> GetMeAsync()
    {
        string? result;

        try
        {
            //var response = await GetGraphServiceClient().Me.GetAsync((config) =>
            //{
            //    // Only request specific properties
            //    config.QueryParameters.Select = ["displayName", "mail", "userPrincipalName"];
            //});

            var response = await GetClient()
                .Me
                .GetAsync();

            result = JsonSerializer.Serialize(response, JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            result = ex.Message;
            //ConsentHandler.HandleException(ex);
        }

        return result;
    }

    public async Task<string?> GetMeUserGroupsAsync()
    {
        string? result;

        try
        {
            var response = await GetClient()
                .Me
                .MemberOf
                .GetAsync();

            result = JsonSerializer.Serialize(response, JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            result = ex.Message;
            //ConsentHandler.HandleException(ex);
        }

        return result;
    }

    public async Task<string?> GetMeInboxAsync()
    {
        string? result;

        try
        {
            var response = await GetClient()
                .Me
                .MailFolders["Inbox"] // Only messages from Inbox folder
                .Messages
                .GetAsync((config) =>
                {
                    /* Only request specific properties */
                    config.QueryParameters.Select = ["from", "isRead", "receivedDateTime", "subject"];
                    /* Get at most 25 results */
                    config.QueryParameters.Top = 25;
                    /* Sort by received time, newest first */
                    config.QueryParameters.Orderby = ["receivedDateTime DESC"];
                });

            result = JsonSerializer.Serialize(response, JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            result = ex.Message;
            //ConsentHandler.HandleException(ex);
        }

        return result;
    }


    public async Task<string?> GetUsersAsync()
    {
        string? result;

        try
        {
            var response = await GetClient()
                .Users
                .GetAsync();

            result = JsonSerializer.Serialize(response, JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            result = ex.Message;
            //ConsentHandler.HandleException(ex);
        }

        return result;
    }

    public async Task<string?> GetUserGroupsAsync(string userId)
    {
        string? result = null;

        try
        {
            var response = await GetClient()
                .Users[userId]
                .MemberOf
                .GetAsync();

            result = JsonSerializer.Serialize(response, JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            result = ex.Message;
            //result = JsonSerializer.Serialize(new ExceptionInfo(ex), JsonSerializerOptions);

            //ConsentHandler.HandleException(ex);
        }

        return result;
    }


    //public async Task<string?> GetUsers(string username)
    //{
    //    string? result = null;

    //    try
    //    {
    //        string[] scopes = new string[] { "user.read" };
    //        var clientApplication = PublicClientApplicationBuilder.Create(_entraIdOptions.ClientId).Build();
    //        var response = await clientApplication.AcquireTokenSilent(scopes, username).ExecuteAsync();
    //        result = response.AccessToken;
    //    }
    //    catch (Exception ex)
    //    {
    //        result = ex.Message;
    //        //ConsentHandler.HandleException(ex);
    //    }

    //    return result;
    //}

    //public async Task<string?> GetMeFromHttpClientAsync(string accessToken)
    //{
    //    string? result = null;

    //    if (GetGraphServiceClient() == null)
    //    {
    //        return result;
    //    }

    //    try
    //    {
    //        var url = "https://graph.microsoft.com/v1.0/me"

    //        // Option1
    //        HttpClient _client = new HttpClient();
    //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(url));
    //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    //        HttpResponseMessage response = await _client.SendAsync(request);
    //        result = await response.Content.ReadAsStringAsync();

    //         // Option 2
    //         HttpClient client = new HttpClient();
    //         client.DefaultRequestHeaders.Add("Authorization", accessToken);
    //         string json = await client.GetStringAsync(url);
    //    }
    //    catch (Exception ex)
    //    {
    //        result = ex.Message;
    //        //ConsentHandler.HandleException(ex);
    //    }

    //    return result;
    //}

    // https://devblogs.microsoft.com/identity/access-cloud-resources-across-tenants-without-secrets-ga/
    // https://devblogs.microsoft.com/identity/access-cloud-resources-across-tenants-without-secrets/
    // https://www.github.com/AzureAD/microsoft-identity-web/wiki/Federated-Identity-Credential-(FIC)-with-a-Managed-Service-Identity-(MSI)
    // https://learn.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme
    // https://learn.microsoft.com/en-us/graph/sdks/choose-authentication-providers

    // https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-config-app-trust-managed-identity?tabs=microsoft-entra-admin-center
    // ClientAssertionCredential
    //var clientAssertionCredential = new ClientAssertionCredential(
    //   _entraId.TenantId,
    //   _entraId.ClientId,
    //   async (token) =>
    //   {
    //	   // fetch Managed Identity token for the specified audience
    //	   var tokenRequestContext = new Azure.Core.TokenRequestContext([_entraId.ClientCredentials[0].TokenExchangeUrl]);
    //	   var accessToken = await new ManagedIdentityCredential(_entraId.ClientCredentials[0].ManagedIdentityClientId)
    //							   .GetTokenAsync(tokenRequestContext)
    //							   .ConfigureAwait(false);
    //	   return accessToken.Token;
    //   });
    //GraphServiceClient = new GraphServiceClient(clientAssertionCredential, _microsoftGraph.Scopes);		

    // ClientSecretCredential
    //await OnGetMicrosoftGraphAsync($"{nameof(ClientAssertionCredential)}-application");
    //GraphServiceClient = new GraphServiceClient(new ClientSecretCredential(_entraId.TenantId, _entraId.ClientId, _entraId.ClientCredentials[0].ClientSecret), _microsoftGraph.Scopes);
    //await OnGetAsync(nameof(ClientSecretCredential));

    // ManagedIdentityCredential An error occurred while processing your request.
    //GraphServiceClient = new GraphServiceClient(new ManagedIdentityCredential(), _microsoftGraph.Scopes);
    //GraphServiceClient = new GraphServiceClient(new ManagedIdentityCredential(_entraId.ClientId), _microsoftGraph.Scopes);
    //GraphServiceClient = new GraphServiceClient(new ManagedIdentityCredential(_entraId.ClientCredentials[0].ManagedIdentityClientId), _microsoftGraph.Scopes);

    // WorkloadIdentityCredential
    //GraphServiceClient = new GraphServiceClient(new WorkloadIdentityCredential(new WorkloadIdentityCredentialOptions
    //{
    //    TenantId = _entraId.TenantId,
    //    ClientId = _entraId.ClientId,
    //    TokenFilePath = _entraId.ClientCredentials[0].TokenExchangeUrl
    //}), _microsoftGraph.Scopes);

    // https://learn.microsoft.com/en-us/entra/msal/dotnet/acquiring-tokens/web-apps-apis/workload-identity-federation
    // ConfidentialClientApplicationBuilder
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
}
