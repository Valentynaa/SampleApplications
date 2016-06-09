using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ClassificationTree
{
    [Serializable]
    public class ClassificationTreesResource
    {
        public IEnumerable<ClassificationTreeResourceSummary> ClassificationTrees;
    }
}