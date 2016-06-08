using System;
using System.Collections.Generic;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;

namespace MagentoConnect.Models.EndlessAisle.ClassificationTree
{
	[Serializable]
	public class ClassificationTreeResource
	{
		public ClassificationTreeResource()
		{
			Categories = new List<ClassificationCategoryResource>();
			Classifications = new List<ClassificationResource>();
		}
		
		public bool IsCanonical { get; set; }
		public string Description { get; set; }
		public EntityRefResource Owner { get; set; }
		public List<ClassificationCategoryResource> Categories { get; set; }
		public List<ClassificationResource> Classifications { get; set; }

		public int Version { get; set; }
	}
}