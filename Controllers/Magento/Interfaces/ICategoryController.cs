using MagentoConnect.Models.Magento.Category;

namespace MagentoConnect.Controllers.Magento
{
	public interface ICategoryController : IController
	{
		CategoryResource GetCategory(int categoryId);
	}
}