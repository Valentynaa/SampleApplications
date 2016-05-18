﻿using System;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Pricing;

namespace MagentoConnect.Mappers
{
	public class PricingMapper : BaseMapper
	{
		private static PricingController _eaPricingController;

		public PricingMapper(string magentoAuthToken, string eaAuthToken) : base(magentoAuthToken, eaAuthToken)
		{
			_eaPricingController = new PricingController(eaAuthToken);
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

			_eaPricingController.CreatePricingResourceItem(new PricingResource(), catalogItemId, price);
		}
	}
}