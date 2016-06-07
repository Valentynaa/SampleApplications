using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.ClassificationTree;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockClassificationTreeController : IClassificationTreeController
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

		public string AuthToken
		{
			get
			{
				return "WHdvc0h9b3NYQW9zWHZvc1h2JSYBAyc_PhRWBSAYLUFsIBkSMEA9MRYVWAcsPCQQaD8NJiBPLSUeFVgXGjEBJA4bX0EaBBtAOR8I";
			}
		}
	}
}
