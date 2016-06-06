using MagentoConnect.Models.EndlessAisle.Availability;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public interface IAvailabilityController : IController
	{
		AvailabilityResource CreateCatalogItem(AvailabilityResource availability);
	}
}