using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class ClassificationBreadcrumbResource
    {
        public int TreeId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<ClassificationBreadcrumbItem> ParentCategories { get; set; }
    }
}
