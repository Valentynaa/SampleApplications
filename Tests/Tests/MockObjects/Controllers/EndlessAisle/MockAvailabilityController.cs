using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Availability;

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
