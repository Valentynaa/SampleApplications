using System;
// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.EndlessAisle.Authentication
{
    [Serializable]
    public class AuthTokenResource
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}