using System;

namespace MagentoConnect.Models.EndlessAisle.ClassificationTree
{
    [Serializable]
    public class ClassificationResource : ClassificationTreeNode
    {
        public object ProductTemplate { get; set; }
        public int Order { get; set; }
    }
}