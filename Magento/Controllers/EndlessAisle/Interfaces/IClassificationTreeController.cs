using MagentoSync.Models.EndlessAisle.ClassificationTree;

namespace MagentoSync.Controllers.EndlessAisle
{
	public interface IClassificationTreeController : IController
	{
		ClassificationTreeResource GetClassificationTree(int classificationTreeId);
		int GetClassificationById(int classificationTreeId, int classificationId);
	}
}