using System;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class RegionRefResource
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }
}