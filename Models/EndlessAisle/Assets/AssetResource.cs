using System;

namespace MagentoConnect.Models.EndlessAisle.Assets
{
    [Serializable]
    public class AssetResource
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string MimeType { get; set; }
        public bool IsHidden { get; set; }
    }
}