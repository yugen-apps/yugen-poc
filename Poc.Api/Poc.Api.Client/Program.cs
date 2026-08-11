using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Poc.Api.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new();

            var tenantId = "";
            var authority = $"https://login.microsoftonline.com/{tenantId}";

            var clientId = "";
            var clientSecret = "";
            var scopes = new[] { $"api://{clientId}/.default" };

            var app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithAuthority(authority)
                .WithClientSecret(clientSecret)
                .Build();

            var basUrl = "https://localhost:8081/api/ado";

            var response = await client.GetAsync($"{basUrl}/anon");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Your response is: {response.StatusCode}");
            Console.WriteLine($"Your content is: {content}");

            //response = await client.GetAsync($"{basUrl}/app");
            //content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"Your response is: {response.StatusCode}");
            //Console.WriteLine($"Your content is: {content}");

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            Console.WriteLine($"Access Token: {result.AccessToken}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            var contentStream = client.GetFromJsonAsAsyncEnumerable<string>($"{basUrl}/stream");
            await foreach (var line in contentStream)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine(line);
                }
            }

            //response = await client.GetAsync($"{basUrl}/anon");
            //content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"Your response is: {response.StatusCode}");
            //Console.WriteLine($"Your content is: {content}");

            //response = await client.GetAsync($"{basUrl}/app");
            //content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"Your response is: {response.StatusCode}");
            //Console.WriteLine($"Your content is: {content}");

            Console.ReadLine();
        }
    }
}
