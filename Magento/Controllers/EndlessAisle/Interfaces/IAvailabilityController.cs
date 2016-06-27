using MagentoSync.Models.EndlessAisle.Availability;

namespace MagentoSync.Controllers.EndlessAisle.Interfaces
{
	public interface IAvailabilityController : IController
	{
		AvailabilityResource CreateCatalogItem(AvailabilityResource availability);
	}
}