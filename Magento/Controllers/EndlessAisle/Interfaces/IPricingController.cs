using MagentoSync.Models.EndlessAisle.Pricing;

namespace MagentoSync.Controllers.EndlessAisle.Interfaces
{
	public interface IPricingController : IController
	{
		/// <summary>
		/// Creates a pricing resource for EA.
		/// </summary>
		/// <param name="regularPrice">Price for the item to be set to.</param>
		/// <param name="catalogItemId">Catalog item to set price for.</param>
		/// <param name="pricing">Object representing Pricing to be created.</param>
		/// <param name="discountable">OPTIONAL: Sets the product as discountable or not.</param>
		/// <returns>Pricing resource created.</returns>
		PricingResource CreatePricingResourceItem(PricingResource pricing, string catalogItemId, decimal regularPrice, bool discountable = false);
	}
}