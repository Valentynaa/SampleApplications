using MagentoSync.Models.Magento.Category;

namespace MagentoSync.Controllers.Magento.Interfaces
{
	public interface ICategoryController : IController
	{
		CategoryResource GetCategory(int categoryId);
	}
}