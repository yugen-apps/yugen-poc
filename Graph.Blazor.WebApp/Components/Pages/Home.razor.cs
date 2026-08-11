using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Graph;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Graph.Blazor.WebApp.Components.Pages
{
    public partial class Home
    {
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
    }
}