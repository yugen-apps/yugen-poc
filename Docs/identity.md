# Identity

## Entra

[Register new app](https://portal.azure.com/#view/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/~/RegisteredApps)

Go to Authentication > Add Redirect URI > Web:
- Redirect URI: https://localhost/signin-oidc (ports aren't required on these URIs). 
- Front-channel logout URL: https://localhost/signout-callback-oidc (a port isn't required). 
Authorized users and groups are assigned to the app's registration in App registrations > Enterprise applications.

In the Entra or Azure portal's Implicit grant and hybrid flows app registration configuration, don't select either checkbox for the authorization endpoint to return Access tokens or ID tokens. The OpenID Connect handler automatically requests the appropriate tokens using the code returned from the authorization endpoint.

add this to appsettings and the related secrets to user secrets

```json
{
  "Authentication": {
    "EntraId": {
      "CallbackPath": "/signin-oidc",
      "ClientId": "{CLIENT ID (BLAZOR APP)}",
      "Domain": "{DIRECTORY NAME}.onmicrosoft.com",
      "Instance": "https://login.microsoftonline.com/",
      "ResponseType": "code",
      "TenantId": "{TENANT ID}"
    }
  }
}
```

## Google

https://console.cloud.google.com/

1. Create a new project if doesn't exist
2. Go to Google Auth > https://console.cloud.google.com/auth/overview > create a new app
3. Go To Clients > https://console.cloud.google.com/auth/clients > create a new client > web
    - Authorised redirect URIs: https://localhost:8080/signin-google
  
add this to appsettings and the related secrets to user secrets

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "{ClientId}",
      "ClientSecret": "{ClientSecret}"
    }
  }
}
```

## MS Graph

In the app's registration screen, select the API permissions blade in the left to open the page where we add access to the APIs that your application needs.
Select the Add a permission button and then,
Ensure that the Microsoft APIs tab is selected.
In the Commonly used Microsoft APIs section, select Microsoft Graph
In the Delegated permissions section, select the User.Read in the list. Use the search box if necessary.
Select the Add permissions button at the bottom.



  # Resources
  
  ## Blazor Security

  https://learn.microsoft.com/en-us/aspnet/core/blazor/security/
  https://learn.microsoft.com/en-us/aspnet/core/blazor/security/authentication-state
  https://learn.microsoft.com/en-us/aspnet/core/blazor/security/blazor-web-app-with-entra
    
  ## Auth / Identity

  https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/
  https://learn.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme  
  https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication/credential-chains
  https://github.com/AzureAD/microsoft-identity-web/wiki
  
  ## MS Graph
  https://learn.microsoft.com/en-us/entra/msidweb/call-downstream-apis/graph-service-client
  https://github.com/microsoftgraph/msgraph-sdk-dotnet  
  https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api
  https://github.com/Azure-Samples/ms-identity-blazor-server/blob/main/WebApp-graph-user/Call-MSGraph/README.md

  https://github.com/AzureAD/microsoft-identity-web/blob/master/src/Microsoft.Identity.Web.MicrosoftGraph/MicrosoftGraphExtensions.cs
  https://github.com/microsoftgraph/msgraph-snippets-dotnet/blob/main/src/SdkSnippets/Snippets/CreateClients.cs
  https://github.com/microsoftgraph/msgraph-training-dotnet/tree/main/user-auth/GraphTutorial

  ## AZ CLI

  https://learn.microsoft.com/en-us/cli/azure/authenticate-azure-cli-managed-identity