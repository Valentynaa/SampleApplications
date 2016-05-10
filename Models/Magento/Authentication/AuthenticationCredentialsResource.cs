using System;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.Authentication
{
    [Serializable]
    public class AuthenticationCredentialsResource
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}