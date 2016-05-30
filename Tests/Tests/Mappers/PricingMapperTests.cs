using System;
using MagentoConnect;
using MagentoConnect.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Mappers
{
    /// <summary>
    /// This suite ensures the PricingMapper is working correctly
    /// </summary>

    [TestClass]
	public class PricingMapperTests
	{
		private PricingMapper _pricingMapper;
		private const double Price = 4.50d;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			var magentoAuthToken = App.GetMagentoAuthToken();
			_pricingMapper = new PricingMapper(magentoAuthToken, eaAuthToken);
		}

		/// <summary>
		/// If this test fails, proper error handling is not in place to deal with a null catalog item ID
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void PricingMapper_UpsertPricingForCatalogItem_InvalidCatalogItemId()
		{
			_pricingMapper.UpsertPricingForCatalogItem(null, (decimal)Price);
		}
	}
}
