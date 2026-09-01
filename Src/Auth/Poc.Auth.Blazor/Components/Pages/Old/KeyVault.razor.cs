using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Poc.Auth.Blazor.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Components.Pages.Old;

public partial class KeyVault
{
    private TokenCredential _azureTokenCredential;

    [Inject]
    public IConfiguration Configuration { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetSecrets();
    }

    private TokenCredential GetAzureTokenCredential()
    {
        var entraId = Configuration.GetSection("EntraId");
        var TenantId = entraId.GetValue<string>("TenantId");
        var ClientId = entraId.GetValue<string>("ClientId");
        var ClientSecret = entraId.GetValue<string>("ClientSecret");

        return _azureTokenCredential ??= new ClientSecretCredential(TenantId, ClientId, ClientSecret);
    }

    private async Task GetSecrets()
    {
        try
        {
            var kvUri = new Uri("https://tmp-key-vault-2.vault.azure.net/");

            //var options = new SecretClientOptions
            //{
            //    Retry =
            //        {
            //            Delay = TimeSpan.FromSeconds(2),
            //            MaxDelay = TimeSpan.FromSeconds(16),
            //            MaxRetries = 5,
            //            Mode = RetryMode.Exponential
            //        }
            //};

            var secretClient = new SecretClient(kvUri, GetAzureTokenCredential());

            var propertiesOfSecrets = secretClient.GetPropertiesOfSecretsAsync();

            var allSecrets = new List<string>();

            await foreach (var secretProperty in propertiesOfSecrets)
            {
                var response = await secretClient.GetSecretAsync(secretProperty.Name);

                allSecrets.Add($"{response.Value.Name}:{response.Value.Value}");
            }

            var secretsString = JsonSerializer.Serialize(allSecrets, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception exception)
        {
            var secretExceptionString = JsonSerializer.Serialize(new ExceptionInfo(exception), new JsonSerializerOptions { WriteIndented = true });
        }
    }
}