using MagentoConnect.Models.EndlessAisle.ClassificationTree;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public interface IClassificationTreeController : IController
	{
		ClassificationTreeResource GetClassificationTree(int classificationTreeId);
		int GetClassificationById(int classificationTreeId, int classificationId);
	}
}