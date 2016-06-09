using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Models.EndlessAisle.Availability;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockAvailabilityController : BaseMockController, IAvailabilityController
	{
		public AvailabilityResource CreateCatalogItem(AvailabilityResource availability)
		{
			return availability;
		}
	}
}
