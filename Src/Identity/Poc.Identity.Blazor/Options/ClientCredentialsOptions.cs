namespace Poc.Identity.Blazor.Options;

public class ClientCredentialsOptions
{
    public string? ClientSecret { get; set; }

    public string? ManagedIdentityClientId { get; set; }

    public string? SourceType { get; set; }

    public string? TokenExchangeUrl { get; set; }
}

/*
Import-Module Microsoft.Graph.Applications

Connect-MgGraph -UseDeviceAuthentication

$servicePrincipalId="6fa5946c-99df-4246-b0a6-9450a7800dcd"

Get-MgServicePrincipalAppRoleAssignedTo -ServicePrincipalId $servicePrincipalId

Get-MgServicePrincipal -Filter "appId eq '6fa5946c-99df-4246-b0a6-9450a7800dcd'"

https://developer.microsoft.com/en-us/graph/graph-explorer

https://graph.microsoft.com/v1.0/servicePrincipals?$filter=appId eq '6fa5946c-99df-4246-b0a6-9450a7800dcd'

https://graph.microsoft.com/v1.0/applications/709ae3ad-7dde-476e-a75b-37b1cb33d79b

{
"requiredResourceAccess": [
    {
        "resourceAppId": "6fa5946c-99df-4246-b0a6-9450a7800dcd",
        "resourceAccess": [
            {
                "id": "311a71cc-e848-46a1-bdf8-97ff7156d8e6",
                "type": "Scope"
            },
            {
                "id": "3afa6a7d-9b1a-42eb-948e-1650a849e176",
                "type": "Role"
            }
        ]
    }
]
}

https://graph.microsoft.com/v1.0/applications/709ae3ad-7dde-476e-a75b-37b1cb33d79b?$select=id,requiredResourceAccess

$TenantID = "5098fa58-735a-4e2f-b5a4-ea9995b7b00a"
$DisplayNameServicePrincpal ="tmp-web-app-blazor"
$GraphAppId = "00000003-0000-0000-c000-000000000000"
$PermissionName = "User.Read.All"

Connect-AzureAD -TenantId $TenantID

$sp = (Get-AzureADServicePrincipal -Filter "displayName eq '$DisplayNameServicePrincpal'")

Write-Host $sp

$GraphServicePrincipal = Get-AzureADServicePrincipal -Filter "appId eq '$GraphAppId'"

$AppRole = $GraphServicePrincipal.AppRoles | Where-Object {$_.Value -eq $PermissionName -and $_.AllowedMemberTypes -contains "Application"}

New-AzureAdServiceAppRoleAssignment -ObjectId $sp.ObjectId -PrincipalId $sp.ObjectId -ResourceId $GraphServicePrincipal.ObjectId -Id $AppRole.Id
*/