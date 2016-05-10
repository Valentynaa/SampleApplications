using System;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ColorTagResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
    }
}