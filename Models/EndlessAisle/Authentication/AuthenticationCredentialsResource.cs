using System;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.EndlessAisle.Authentication
{
    [Serializable]
    public class AuthenticationCredentialsResource
    {
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}