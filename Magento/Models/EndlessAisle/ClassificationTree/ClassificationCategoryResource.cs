using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ClassificationTree
{
    [Serializable]
    public class ClassificationCategoryResource : ClassificationTreeNode
    {
        public List<ClassificationCategoryResource> Categories { get; set; }
        public List<ClassificationResource> Classifications { get; set; }
        public int Order { get; set; }
    }
}