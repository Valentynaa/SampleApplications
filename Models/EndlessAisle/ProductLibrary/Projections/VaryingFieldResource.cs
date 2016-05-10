using System;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class VaryingFieldResource
    {
        public int FieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}