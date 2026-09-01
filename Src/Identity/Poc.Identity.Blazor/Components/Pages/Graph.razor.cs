using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Poc.Identity.Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Poc.Identity.Blazor.Components.Pages;

public partial class Graph
{
    private TokenCredential _azureTokenCredential;


    [Inject]
    private ITokenAcquisition TokenAcquisition { get; set; }

    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject]
    public IAuthorizationHeaderProvider AuthorizationHeaderProvider { get; set; }

    [Inject]
    public GraphServiceClient GraphServiceClient { get; set; }


    //private ChainedTokenCredential? _azureTokenCredential = null;

    [Inject]
    public IConfiguration Configuration { get; set; }

    [Inject]
    MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; }

    public string graphExceptionString { get; private set; }

    public string graphUserGroupsString { get; private set; }

    public string graphUserString { get; private set; }

    public string secretExceptionString { get; private set; }

    public string secretsString { get; private set; }

    public string userIdentityString { get; private set; }

    //		public ChainedTokenCredential GetAzureTokenCredential()
    //		{
    //			if (_azureTokenCredential != null)
    //			{
    //				return _azureTokenCredential;
    //			}

    //			// https://learn.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme?view=azure-dotnet
    //			var tenantId = Configuration.GetSection("EntraId")["TenantId"];
    //			var instance = Configuration.GetSection("EntraId")["Instance"];
    //			var clientId = Configuration.GetSection("EntraId")["ClientId"];

    //#if DEBUG

    //			return _azureTokenCredential = new ChainedTokenCredential(
    //				//new VisualStudioCredential()
    //				//new AzureCliCredential()
    //				//new VisualStudioCodeCredential()
    //				new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions()
    //				{
    //					TokenCachePersistenceOptions = new TokenCachePersistenceOptions(),
    //					TenantId = tenantId
    //				})
    //			);
    //#else
    //			return _azureTokenCredential = new ChainedTokenCredential(new ManagedIdentityCredential(clientId));
    //#endif
    //		}

    protected override async Task OnInitializedAsync()
    {
        await GetAuth();

        await GetGraph();

        await GetSecrets();

        //await GetPipelines();


        await GetClaimsPrincipalData();

        await GetUserProfile();

        await GetUsers();

        //await GetToken();

        //await GetToken2();

        await GetSecrets2();
    }

    private async Task GetAuth()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var authStateUser = authState.User;

        if (authStateUser.Identity != null &&
            authStateUser.Identity.IsAuthenticated)
        {
            var userIdentity = authStateUser.Identity;
            userIdentityString = JsonSerializer.Serialize(userIdentity, new JsonSerializerOptions { WriteIndented = true });
        }
        try
        {
            //string[] scopes = new string[] { "user.read" };

            //var clientApplication = PublicClientApplicationBuilder.Create("dfcca165-b5a4-449b-a245-129f0ef09937").Build();

            //var authResult = await clientApplication.AcquireTokenSilent(scopes, authStateUser.Identity.Name).ExecuteAsync();

            //string authorizationHeader = await AuthorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(scopes);

            var request = await GraphServiceClient.Me.GetAsync();

            //string[] scopes = new string[] { "user.read", "User.ReadBasic.All" };
            //var graphClient = new GraphServiceClient(new DefaultAzureCredential(), scopes);

            //var users = await GraphServiceClient.Users.GetAsync();

            //authResult = await app.AcquireTokenSilent(scopes, firstAccount)
            //									  .ExecuteAsync();


            // Acquire the access token.
            //string accessToken = await AuthorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(scopes);

            // Use the access token to call a protected web API.
            //HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", accessToken);
            //string json = await client.GetStringAsync(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ConsentHandler.HandleException(ex);
        }
    }

    private async Task GetGraph()
    {
        //try
        //{
        //	var scopes = new[] { "https://graph.microsoft.com/.default" };
        //	var graphClient = new GraphServiceClient(GetAzureTokenCredential(), scopes);

        //	var graphUser = await graphClient.Me.GetAsync();
        //	graphUserString = JsonSerializer.Serialize(graphUser, new JsonSerializerOptions { WriteIndented = true });

        //	var graphUserGroups = await graphClient.Users[graphUser.Id].MemberOf.GetAsync();
        //	graphUserGroupsString = JsonSerializer.Serialize(graphUserGroups, new JsonSerializerOptions { WriteIndented = true });
        //}
        //catch (Exception exception)
        //{
        //	graphExceptionString = JsonSerializer.Serialize(new ExceptionInfo(exception), new JsonSerializerOptions { WriteIndented = true });
        //}
    }

    private async Task GetPipelines()
    {
    }

    private async Task GetSecrets()
    {
        //	try
        //	{
        //		var kvUri = new Uri("https://tmp-key-vault-2.vault.azure.net/");

        //		var options = new SecretClientOptions
        //		{
        //			Retry =
        //			{
        //				Delay = TimeSpan.FromSeconds(2),
        //				MaxDelay = TimeSpan.FromSeconds(16),
        //				MaxRetries = 5,
        //				Mode = RetryMode.Exponential
        //			}
        //		};

        //		var secretClient = new SecretClient(kvUri, GetAzureTokenCredential(), options);

        //		var propertiesOfSecrets = secretClient.GetPropertiesOfSecretsAsync();

        //		var allSecrets = new List<string>();

        //		await foreach (var secretProperty in propertiesOfSecrets)
        //		{
        //			var response = await secretClient.GetSecretAsync(secretProperty.Name);

        //			allSecrets.Add($"{response.Value.Name}:{response.Value.Value}");
        //		}

        //		secretsString = JsonSerializer.Serialize(allSecrets, new JsonSerializerOptions { WriteIndented = true });
        //	}
        //	catch (Exception exception)
        //	{
        //		secretExceptionString = JsonSerializer.Serialize(new ExceptionInfo(exception), new JsonSerializerOptions { WriteIndented = true });
        //	}
    }

    private TokenCredential GetAzureTokenCredential()
    {
        if (_azureTokenCredential != null)
        {
            return _azureTokenCredential;
        }

        var entraId = Configuration.GetSection("EntraId");
        var TenantId = entraId.GetValue<string>("TenantId");
        var ClientId = entraId.GetValue<string>("ClientId");
        var ClientSecret = entraId.GetValue<string>("ClientSecret");

        _azureTokenCredential = new ClientSecretCredential(TenantId, ClientId, ClientSecret);

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

        return _azureTokenCredential;
    }

    private async Task GetClaimsPrincipalData()
    {
        // Gets an AuthenticationState that describes the current user.
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        var authStateUser = authState.User;

        // Checks if the user has been authenticated.
        if (authStateUser.Identity != null &&
            authStateUser.Identity.IsAuthenticated)
        {
            var userIdentity = authStateUser.Identity;

            // Sets the claims value in _claims variable.
            // The claims mentioned in printClaims variable are selected only.
            string[] printClaims = { "name", "preferred_username", "tid", "oid" };
            var claims = authStateUser.Claims.Where(x => printClaims.Contains(x.Type));

            userIdentityString = JsonSerializer.Serialize(userIdentity, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    private async Task GetSecrets2()
    {
        try
        {
            var kvUri = new Uri("https://tmp-key-vault-2.vault.azure.net/");

            var secretClient = new SecretClient(kvUri, GetAzureTokenCredential());

            var propertiesOfSecrets = secretClient.GetPropertiesOfSecretsAsync();

            var allSecrets = new List<string>();

            await foreach (var secretProperty in propertiesOfSecrets)
            {
                var response = await secretClient.GetSecretAsync(secretProperty.Name);

                allSecrets.Add($"{response.Value.Name}:{response.Value.Value}");
            }

            secretsString = JsonSerializer.Serialize(allSecrets, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception exception)
        {
            secretExceptionString = JsonSerializer.Serialize(new ExceptionInfo(exception), new JsonSerializerOptions { WriteIndented = true });
        }
    }

    private async Task GetToken()
    {
        try
        {
            var scopes = Configuration.GetSection("Graph:Scopes").Get<string[]>();
            string accessToken = await AuthorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(scopes);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ConsentHandler.HandleException(ex);
        }
    }

    private async Task GetToken2()
    {
        try
        {
            //define the scope
            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

            //Getting token from Azure Active Directory
            string accessToken = await TokenAcquisition.GetAccessTokenForUserAsync(scopes);

            //Request Grap API end point
            //HttpClient _client = new HttpClient();
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("https://graph.microsoft.com/v1.0/me"));
            ////Passing Token For this Request
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //HttpResponseMessage response = await _client.SendAsync(request);
            //Get User into from grpah API
            //dynamic userInfo = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ConsentHandler.HandleException(ex);
        }
    }

    private async Task GetUserProfile()
    {
        try
        {
            var graphUser = await GraphServiceClient.Me.GetAsync();
            graphUserString = JsonSerializer.Serialize(graphUser, new JsonSerializerOptions { WriteIndented = true });

            var graphUserGroups = await GraphServiceClient.Me.MemberOf.GetAsync();

            var roles = graphUserGroups?.Value?.Select(x => (x as Group)?.DisplayName)
                            .Where(x => x != null &&
                                        x.StartsWith("ado-group"))
                            .ToList() ?? [];
            graphUserGroupsString = JsonSerializer.Serialize(graphUserGroups, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ConsentHandler.HandleException(ex);

            graphExceptionString = JsonSerializer.Serialize(new ExceptionInfo(ex), new JsonSerializerOptions { WriteIndented = true });
        }
    }

    private async Task GetUsers()
    {
        try
        {
            var users = await GraphServiceClient.Users.GetAsync();
            //graphUserGroupsString = JsonSerializer.Serialize(graphUserGroups, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ConsentHandler.HandleException(ex);
        }
    }
}