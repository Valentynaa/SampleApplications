using MagentoSync.Models.Magento.CustomAttributes;

namespace MagentoSync.Controllers.Magento
{
	public interface ICustomAttributesController : IController
	{
		CustomAttributeResource GetCustomAttributeIfExists(string attributeCode);
		CustomAttributeResource CreateCustomAttribute(string attributeCode, string inputType, bool isRequired, string label);
	}
}