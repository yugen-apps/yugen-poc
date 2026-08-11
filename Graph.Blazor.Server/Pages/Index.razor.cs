using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Graph.Blazor.Server.Pages
{
    public partial class Index
    {
        private TokenCredential _azureTokenCredential;

        public string graphExceptionString { get; private set; }

        public string graphUserGroupsString { get; private set; }

        public string graphUserString { get; private set; }

        public string secretExceptionString { get; private set; }

        public string secretsString { get; private set; }

        public string userIdentityString { get; private set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        private IAuthorizationHeaderProvider AuthorizationHeaderProvider { get; set; }

        [Inject]
        private IConfiguration Configuration { get; set; }

        [Inject]
        private MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; }

        [Inject]
        private GraphServiceClient GraphClient { get; set; }

        [Inject]
        private ITokenAcquisition TokenAcquisition { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetClaimsPrincipalData();

            await GetUserProfile();

            await GetUsers();

            //await GetToken();

            //await GetToken2();

            await GetSecrets();
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

        private async Task GetSecrets()
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
                var graphUser = await GraphClient.Me.GetAsync();
                graphUserString = JsonSerializer.Serialize(graphUser, new JsonSerializerOptions { WriteIndented = true });

                var graphUserGroups = await GraphClient.Me.MemberOf.GetAsync();

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
                var users = await GraphClient.Users.GetAsync();
                //graphUserGroupsString = JsonSerializer.Serialize(graphUserGroups, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ConsentHandler.HandleException(ex);
            }
        }
    }
}