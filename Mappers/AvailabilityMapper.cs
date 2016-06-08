using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Availability;
using MagentoConnect.Utilities;
using System;

namespace MagentoConnect.Mappers
{
	public class AvailabilityMapper : BaseMapper
	{
		private static IAvailabilityController _eaAvailabilityController;

		public AvailabilityMapper(IAvailabilityController availabilityController)
		{
			_eaAvailabilityController = availabilityController;
		}

		/// <summary>
		/// Upserts an EA availability / quantity record for a catalog product. EA requires products to have availability
		/// before they can show up.
		/// 
		/// Availability is set at the COMPANY level in EA
		/// 
		/// NOTE:
		///		By default, this function creates an availability record of 1 for a product across the company
		///		Allowing a product to appear in EA, passing the "do not display out of stock products" rule.
		/// </summary>
		/// <param name="catalogItemId">Item ID for EA catalog</param>
		/// <param name="quantity">Quantity to set for the item. Defaults to 1 if not set.</param>
		public void UpsertAvailabilityForCatalogItem(string catalogItemId, int quantity = 1)
		{
			if (catalogItemId == null)
			{
				throw new Exception("A catalog item ID must be provided.");
			}

			var availability = new AvailabilityResource
			{
				Id = new Guid(catalogItemId),
				EntityId = ConfigReader.EaCompanyId,
				Quantity = quantity
			};

			_eaAvailabilityController.CreateCatalogItem(availability);
		}
	}
}
