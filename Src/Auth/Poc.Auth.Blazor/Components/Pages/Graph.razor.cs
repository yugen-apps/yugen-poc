using Microsoft.AspNetCore.Components;
using Poc.Auth.Blazor.Services.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Components.Pages;

public partial class Graph
{
    private Dictionary<string, string?> _appList = [];
    private Dictionary<string, string?> _userList = [];

    [Inject]
    private AppGraphService AppGraphService { get; set; } = null!;

    [Inject]
    private UserGraphService UserGraphService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _appList = await InitializeAsync(AppGraphService);

        _userList = await InitializeAsync(UserGraphService);

        await base.OnInitializedAsync();
    }

    private async Task<Dictionary<string, string?>> InitializeAsync(BaseGraphService graphService)
    {
        Dictionary<string, string?> list = [];

        var tokenFromAzureTokenCredential = await graphService.GetTokenFromAzureTokenCredentialAsync();
        list.Add("tokenFromAzureTokenCredential", tokenFromAzureTokenCredential);

        //var tokenFromAuthorizationHeaderProvider = await GraphService.GetTokenFromAuthorizationHeaderProviderAsync();
        //_list.Add("tokenFromAuthorizationHeaderProvider", tokenFromAuthorizationHeaderProvider);

        //var tokenFromTokenAcquisition = await GraphService.GetTokenFromTokenAcquisitionAsync();
        //_list.Add("tokenFromTokenAcquisition", tokenFromTokenAcquisition);

        string? userId = null;

        var user = await graphService.GetMeAsync();
        list.Add("user", user);

        try
        {
            using var jsonDocument = JsonDocument.Parse(user ?? string.Empty);

            userId = jsonDocument
                .RootElement
                .GetProperty("id")
                .GetString();
        }
        catch (Exception ex)
        {
            // ignored
        }


        var userGroups = await graphService.GetMeUserGroupsAsync();
        list.Add("userGroups", userGroups);

        var inbox = await graphService.GetMeInboxAsync();
        list.Add("inbox", inbox);

        var users = await graphService.GetUsersAsync();
        list.Add("users", users);

        try
        {
            using var jsonDocument = JsonDocument.Parse(users ?? string.Empty);

            userId = jsonDocument
                .RootElement
                .EnumerateObject()
                .First()
                .Value
                .GetProperty("id")
                .GetString();
        }
        catch (Exception ex)
        {
            // ignored
        }

        list.Add("userId", userId);


        if (!string.IsNullOrWhiteSpace(userId))
        {
            var userGroups2 = await graphService.GetUserGroupsAsync(userId);
            list.Add("userGroups2", userGroups2);
        }

        return list;
    }
}