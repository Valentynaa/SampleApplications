using MagentoSync.Models.EndlessAisle.Availability;

namespace MagentoSync.Controllers.EndlessAisle
{
	public interface IAvailabilityController : IController
	{
		AvailabilityResource CreateCatalogItem(AvailabilityResource availability);
	}
}