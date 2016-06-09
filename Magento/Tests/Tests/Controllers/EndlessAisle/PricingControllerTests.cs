using MagentoSync;
using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Models.EndlessAisle.Pricing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.EndlessAisle
{
	/// <summary>
	/// This suite ensures the PricingController is working correctly
	/// </summary>
	[TestClass]
	public class PricingControllerTests
	{
		//IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from Endless Aisle
		private const string CatalogItemId = "d4c4e5c7-0ce7-4e58-bf5c-8664b41b54de";

		private PricingController _pricingController;
		private const double Price = 4.50d;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			_pricingController = new PricingController(eaAuthToken);
		}

		/// <summary>
		/// If this test fails, the CreatePricingResourceItem did not update the price in EA.
		/// 
		/// Product updated is determined by the catalogItemId
		/// </summary>
		[TestMethod]
		public void PricingController_CreatePricingResourceItem()
		{
			Assert.IsNotNull(_pricingController.CreatePricingResourceItem(new PricingResource(), CatalogItemId, (decimal)Price));
		}

	}
}
