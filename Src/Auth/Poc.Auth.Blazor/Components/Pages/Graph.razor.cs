using Azure;
using Microsoft.AspNetCore.Components;
using Poc.Auth.Blazor.Services.Graph;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Components.Pages;

public partial class Graph
{
    private Dictionary<string, string> _list = [];

    [Inject]
    private GraphService GraphService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var tokenFromAzureTokenCredential = await GraphService.GetTokenFromAzureTokenCredentialAsync();
        _list.Add("tokenFromAzureTokenCredential", tokenFromAzureTokenCredential);

        //var tokenFromAuthorizationHeaderProvider = await GraphService.GetTokenFromAuthorizationHeaderProviderAsync();
        //_list.Add("tokenFromAuthorizationHeaderProvider", tokenFromAuthorizationHeaderProvider);

        //var tokenFromTokenAcquisition = await GraphService.GetTokenFromTokenAcquisitionAsync();
        //_list.Add("tokenFromTokenAcquisition", tokenFromTokenAcquisition);

        var user = await GraphService.GetMeAsync();
        _list.Add("user", user);

        using var jsonDocument = JsonDocument.Parse(user);

        var userId = jsonDocument
            .RootElement
            .GetProperty("id")
            .GetString();

        var userGroups = await GraphService.GetMeUserGroupsAsync();
        _list.Add("userGroups", userGroups);

        var inbox = await GraphService.GetMeInboxAsync();
        _list.Add("inbox", inbox);

        var users = await GraphService.GetUsersAsync();
        //_list.Add("inbox", users);

        var userGroups2 = await GraphService.GetUserGroupsAsync(userId);
        _list.Add("userGroups2", userGroups2);

        await base.OnInitializedAsync();
    }
}