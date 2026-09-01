# Identity

## Entra

[Register new app](https://portal.azure.com/#view/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/~/RegisteredApps)

Go to Authentication > Add Redirect URI > Web:
- Redirect URI: https://localhost/signin-oidc (ports aren't required on these URIs). 
- Front-channel logout URL: https://localhost/signout-callback-oidc (a port isn't required). 
Authorized users and groups are assigned to the app's registration in App registrations > Enterprise applications.

In the Entra or Azure portal's Implicit grant and hybrid flows app registration configuration, don't select either checkbox for the authorization endpoint to return Access tokens or ID tokens. The OpenID Connect handler automatically requests the appropriate tokens using the code returned from the authorization endpoint.

add this to appsettings and the related secrets to user secrets

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

  ## Google

  https://console.cloud.google.com/

  1. Create a new project if doesn't exist
  2. Go to Google Auth > https://console.cloud.google.com/auth/overview > create a new app
  3. Go To Clients > https://console.cloud.google.com/auth/clients > create a new client > web
    - Authorised redirect URIs: https://localhost:8080/signin-google
  
add this to appsettings and the related secrets to user secrets

{
  "Authentication": {
    "Google": {
      "ClientId": "{ClientId}",
      "ClientSecret": "{ClientSecret}"
    }
  }
}




  # Resources
  
  https://learn.microsoft.com/en-us/aspnet/core/blazor/security/
  https://learn.microsoft.com/en-us/aspnet/core/blazor/security/authentication-state
  https://learn.microsoft.com/en-us/aspnet/core/blazor/security/blazor-web-app-with-entra
  
  https://learn.microsoft.com/en-us/cli/azure/authenticate-azure-cli-managed-identity

  https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/