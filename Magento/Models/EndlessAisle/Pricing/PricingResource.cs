using System;

namespace MagentoSync.Models.EndlessAisle.Pricing
{
	/// <summary>
	/// Pricing information provides pricing details for a given product.
	/// </summary>
	[Serializable]
	public class PricingResource
	{
		/// <summary>
		/// Unique Identifier for the PricingResource.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The companyId to which the pricing belongs.
		/// </summary>
		public int CompanyId { get; set; }

		/// <summary>
		/// EntityId to which the CatalogProduct belongs.
		/// </summary>
		public int EntityId { get; set; }

		/// <summary>
		/// Id to which the item belongs.
		/// </summary>
		public Guid CatalogItemId { get; set; }

		/// <summary>
		/// The PricingTermId associated with the PricingResource.
		/// </summary>
		public int? PricingTermId { get; set; }

		/// <summary>
		/// The Regular price for the CatalogProductId.
		/// </summary>
		public decimal RegularPrice { get; set; }

		/// <summary>
		/// The Override price for the CatalogProductId.
		/// </summary>
		public decimal? OverridePrice { get; set; }

		/// <summary>
		/// A flag that says if the price can potentially be discounted for the product.
		/// </summary>
		public bool IsDiscountable { get; set; }

		/// <summary>
		/// The minimum price to charge for an item. If null, there is no floor price set.
		/// </summary>
		public decimal? FloorPrice { get; set; }

		/// <summary>
		/// Optional field. Used to explicitly show the original price of an item, should it need to be displayed with the regular and sale/override prices.
		/// </summary>
		public decimal? OriginalPrice { get; set; }

		/// <summary>
		/// If there is an override price associated to this record, it is defined here. If supplied, on record upate, it will attempt to update the saleovveride price to the specified value.
		/// </summary>
		public int? OverridePriceId { get; set; }
	}
}
