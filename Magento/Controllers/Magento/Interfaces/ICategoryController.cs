using MagentoSync.Models.Magento.Category;

namespace MagentoSync.Controllers.Magento
{
	public interface ICategoryController : IController
	{
		CategoryResource GetCategory(int categoryId);
	}
}