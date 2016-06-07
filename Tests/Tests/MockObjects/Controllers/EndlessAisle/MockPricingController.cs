using System;
using System.Collections.Generic;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Pricing;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockPricingController : IPricingController
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

		public string AuthToken
		{
			get
			{
				return "WHdvc0h9b3NYQW9zWHZvc1h2JSYBAyc_PhRWBSAYLUFsIBkSMEA9MRYVWAcsPCQQaD8NJiBPLSUeFVgXGjEBJA4bX0EaBBtAOR8I";
			}
		}
	}
}
