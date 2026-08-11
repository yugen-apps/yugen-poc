namespace Entra.Blazor.Models
{
    public class EntraId
    {
        public string CallbackPath { get; set; }

        public ClientCredentials[] ClientCredentials { get; set; }

        public string ClientId { get; set; }

        public string Domain { get; set; }

        public string Instance { get; set; }

        public string TenantId { get; set; }
    }
}