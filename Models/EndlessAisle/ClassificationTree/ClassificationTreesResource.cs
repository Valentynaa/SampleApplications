using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ClassificationTree
{
    [Serializable]
    public class ClassificationTreesResource
    {
        public IEnumerable<ClassificationTreeResourceSummary> ClassificationTrees;
    }
}