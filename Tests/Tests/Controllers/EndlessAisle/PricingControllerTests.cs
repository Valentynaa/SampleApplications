using System;
using MagentoConnect;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Pricing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.EndlessAisle
{
	[TestClass]
	public class PricingControllerTests
	{
		//Private variables go here
		private PricingController _pricingController;
		private const string CatalogItemId = "d4c4e5c7-0ce7-4e58-bf5c-8664b41b54de";
		private const double Price = 4.50d;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			_pricingController = new PricingController(eaAuthToken);
		}

		//Tests go here

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
