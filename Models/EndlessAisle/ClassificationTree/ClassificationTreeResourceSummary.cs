using System;
using System.Collections.Generic;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;

namespace MagentoConnect.Models.EndlessAisle.ClassificationTree
{
    [Serializable]
    public class ClassificationTreeResourceSummary
    {
        public ClassificationTreeResourceSummary()
        {
            Classifications = new List<ClassificationResource>();
            Categories = new List<ClassificationCategoryResource>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCanonical { get; set; }
        public string Description { get; set; }
        public EntityRefResource Owner { get; set; }

        public int Version { get; set; }
        public DateTime? LastModifiedUtc { get; set; }

        public List<ClassificationResource> Classifications { get; set; }
        public List<ClassificationCategoryResource> Categories { get; set; }
    }
}