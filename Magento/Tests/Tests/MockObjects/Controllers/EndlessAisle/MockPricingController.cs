using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Models.EndlessAisle.Pricing;
using System;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockPricingController : BaseMockController, IPricingController
	{
		public PricingResource CreatePricingResourceItem(PricingResource pricing, string catalogItemId, decimal regularPrice, bool discountable = false)
		{
			pricing.RegularPrice = regularPrice;
			pricing.CatalogItemId = new Guid(catalogItemId);
			pricing.CompanyId = 14146;
			pricing.EntityId = 14192;
			pricing.IsDiscountable = discountable;
			return pricing;
		}
	}
}
