using System;
using MagentoConnect;
using MagentoConnect.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Mappers
{
	[TestClass]
	public class PricingMapperTests
	{
		//Private variables go here
		private PricingMapper _pricingMapper;
		private const string CatalogItemId = "d4c4e5c7-0ce7-4e58-bf5c-8664b41b54de";
		private const double Price = 4.50d;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_pricingMapper = new PricingMapper(magentoAuthToken, eaAuthToken);
		}

		//Tests go here

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
