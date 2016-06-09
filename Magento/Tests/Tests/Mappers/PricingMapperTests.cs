using System;
using MagentoSync;
using MagentoSync.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.MockObjects.Controllers.EndlessAisle;

namespace Tests.Mappers
{
	/// <summary>
	/// This suite ensures the PricingMapper is working correctly
	/// 
	/// NOTE:
	///		This class does NOT use actual calls to the APIs and instead relies on mock controllers
	/// </summary>

	[TestClass]
	public class PricingMapperTests
	{
		private PricingMapper _pricingMapper;
		private const double Price = 4.50d;

		[TestInitialize]
		public void SetUp()
		{
			_pricingMapper = new PricingMapper(new MockPricingController());
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
