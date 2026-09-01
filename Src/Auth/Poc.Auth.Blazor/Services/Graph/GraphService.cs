using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Poc.Auth.Blazor.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Services.Graph;

public class GraphService
{
    private static JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    private readonly EntraIdOptions _entraIdOptions;
    private readonly MicrosoftGraphOptions _microsoftGraphOptionsOptions;
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly IAuthorizationHeaderProvider _authorizationHeaderProvider;
    private readonly ILogger<GraphService> _logger;

	private GraphServiceClient? _userClient;
    private ChainedTokenCredential? _azureTokenCredential;

	public GraphService(
		IOptions<EntraIdOptions> entraIdOptions,
		IOptions<MicrosoftGraphOptions> microsoftGraphOptionsOptions,
		ILogger<GraphService> logger)
	{
		_entraIdOptions = entraIdOptions.Value;
		_microsoftGraphOptionsOptions = microsoftGraphOptionsOptions.Value;
		_logger = logger;
	}

	public GraphServiceClient GetUserGraphServiceClient()
    {
        return _userClient ??= new GraphServiceClient(GetAzureTokenCredential(), _microsoftGraphOptionsOptions.UserScopes);
    }

    //public GraphServiceClient GetAppGraphServiceClient()
    //{
    //    // Use the default scope, which will request the scopes configured on the app registration
    //    string[] scopes = ["https://graph.microsoft.com/.default"];
    //    return _userClient ??= new GraphServiceClient(GetAzureTokenCredential(), scopes);
    //}


    public async Task<string?> GetTokenFromAzureTokenCredentialAsync()
    {
        string? result = null;

        if (GetAzureTokenCredential() == null)
        {
            return result;
        }

        // Request token with given scopes
        var context = new TokenRequestContext(_microsoftGraphOptionsOptions.UserScopes);
        var response = await _azureTokenCredential.GetTokenAsync(context);
        result = response.Token;

        return result;
    }

    public async Task<string?> GetTokenFromAuthorizationHeaderProviderAsync()
    {
        string? result = null;

        try
        {
            result = await _authorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(_microsoftGraphOptionsOptions.UserScopes);
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
        string? result = null;

        try
        {
            // Getting token from Azure Active Directory
            result = await _tokenAcquisition.GetAccessTokenForUserAsync(_microsoftGraphOptionsOptions.UserScopes);
        }
        catch (Exception ex)
        {
            result = ex.Message;
            //ConsentHandler.HandleException(ex);
        }

        return result;
    }

    private ChainedTokenCredential GetAzureTokenCredential()
    {
        // user
        return _azureTokenCredential ??= new ChainedTokenCredential(
            new AzureCliCredential()
            //new DeviceCodeCredential(),
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

        //_azureTokenCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        //{
        //	ExcludeAzureCliCredential = true,
        //	ExcludeAzureDeveloperCliCredential = true,
        //	ExcludeAzurePowerShellCredential = true,
        //	ExcludeInteractiveBrowserCredential = true,
        //	ExcludeVisualStudioCodeCredential = true,
        //	ExcludeVisualStudioCredential = true,

        //	ExcludeEnvironmentCredential = false,
        //	ExcludeManagedIdentityCredential = false,
        //	ExcludeSharedTokenCacheCredential = false,
        //	ExcludeWorkloadIdentityCredential = false
        //});

        // app
        // return _azureTokenCredential = new ChainedTokenCredential(new ManagedIdentityCredential(clientId));
    }


    public async Task<string?> GetMeAsync()
    {
        string? result = null;

        if (GetUserGraphServiceClient() == null)
        {
            return result;
        }

        try
        {
            //var response = await GetGraphServiceClient().Me.GetAsync((config) =>
            //{
            //    // Only request specific properties
            //    config.QueryParameters.Select = ["displayName", "mail", "userPrincipalName"];
            //});

            var response = await GetUserGraphServiceClient()
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
        string? result = null;

        if (GetUserGraphServiceClient() == null)
        {
            return result;
        }

        try
        {
            var response = await GetUserGraphServiceClient()
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
        string? result = null;

        if (GetUserGraphServiceClient() == null)
        {
            return result;
        }

        try
        {
            var response = await GetUserGraphServiceClient()
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
        string? result = null;

        if (GetUserGraphServiceClient() == null)
        {
            return result;
        }

        try
        {
            var response = await GetUserGraphServiceClient()
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

        if (GetUserGraphServiceClient() == null)
        {
            return result;
        }

        try
        {
            var response = await GetUserGraphServiceClient()
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

    //private DeviceCodeCredential GetDeviceCodeCredential(
    //    Func<DeviceCodeInfo, CancellationToken, Task> deviceCodePrompt)
    //{
    //    var options = new DeviceCodeCredentialOptions
    //    {
    //        ClientId = _entraIdOptions.ClientId,
    //        TenantId = _entraIdOptions.TenantId,
    //        DeviceCodeCallback = deviceCodePrompt,
    //    };

    //    return new DeviceCodeCredential(options);
    //}
}
