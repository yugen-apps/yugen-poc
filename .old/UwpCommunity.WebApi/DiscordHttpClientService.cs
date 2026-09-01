using System.Net.Http;

namespace UwpCommunity.WebApi.Services
{
    public class DiscordHttpClientService 
    {
        private readonly string url = "https://discordapp.com/api";
        private readonly HttpClient httpClient = new HttpClient();

        public string GetDiscordUser(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);

            var response = httpClient.GetAsync($"{url}/v6/users/@me").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            // var discordUser = JsonSerializer.Deserialize<DiscordUser>(content);
            return response.IsSuccessStatusCode ? 
                    "discordUser" :
                    string.Empty;
        }
    }
}
