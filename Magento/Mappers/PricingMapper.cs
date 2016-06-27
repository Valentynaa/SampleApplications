using System;
using MagentoSync.Controllers.EndlessAisle.Interfaces;
using MagentoSync.Models.EndlessAisle.Pricing;

namespace MagentoSync.Mappers
{
	public class PricingMapper : BaseMapper
	{
		private static IPricingController _eaPricingController;

		public PricingMapper(IPricingController pricingController)
		{
			_eaPricingController = pricingController;
		}
		
		/// <summary>
		/// Creates a pricing upsert for the price of a catalog item in EA.
		/// </summary>
		/// <param name="catalogItemId">Identifier for catalog item to upsert pricing for</param>
		/// <param name="price">Price to upsert</param>
		public void UpsertPricingForCatalogItem(string catalogItemId, decimal price)
		{
			if (catalogItemId == null)
				throw new Exception("A catalogItemId is required to upsert pricing.");

			_eaPricingController.CreatePricingResourceItem(new PricingResource(), catalogItemId, price, false);
		}
	}
}
