using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class SpecificationFieldResource
    {
        public int Id { get; set; }

        public string StringId { get; set; }
        public string DisplayName { get; set; }

        [Obsolete("Use StringId or DisplayName instead.")]
        public string Name { get; set; }

        public string Value { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
    }
}