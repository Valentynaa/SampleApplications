using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.ClassificationTree;
using System.Collections.Generic;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockClassificationTreeController : BaseMockController, IClassificationTreeController
	{
		public ClassificationTreeResource GetClassificationTree(int classificationTreeId)
		{
			return new ClassificationTreeResource()
			{
				IsCanonical = false,
				Categories = new List<ClassificationCategoryResource>(),
				Classifications = new List<ClassificationResource>(),
				Description = "Test tree description",
				Owner = null
			};
		}

		public int GetClassificationById(int classificationTreeId, int classificationId)
		{
			return 1;
		}
	}
}
