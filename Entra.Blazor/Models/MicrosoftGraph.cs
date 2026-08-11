namespace Entra.Blazor.Models
{
    public class MicrosoftGraph
    {
        public string BaseUrl { get; set; }

        public bool RequestAppToken { get; set; }

        public string[] Scopes { get; set; }
    }
}