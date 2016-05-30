using System;
using System.Collections.Generic;
using System.Linq;
using MagentoConnect;
using MagentoConnect.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Mappers
{
    /// <summary>
    /// This suite ensures the ProductMapper is working correctly
    /// </summary>
	[TestClass]
	public class ProductMapperTests
	{
        //IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from your Magento system
        private const int MagentoProductId = 1;
        private const string MagentoProductSku = "24-MB01";
        private const int MagentoCategoryId = 4;
        private const int MagentoProductQuantity = 100;

        private ProductMapper _productMapper;

        [TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			var magentoAuthToken = App.GetMagentoAuthToken();
			_productMapper = new ProductMapper(magentoAuthToken, eaAuthToken);
			TestHelper.CreateTestUpdate(magentoAuthToken, MagentoProductId, MagentoProductSku, new List<int> { MagentoCategoryId });
		}

		/// <summary>
		/// This test to ensures that the ProductMapper will not check updates from the future.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ProductMapper_GetMagentoProductsUpdatedAfter_WithUpdateFromFuture()
		{
			_productMapper.GetMagentoProductsUpdatedAfter(DateTime.UtcNow.AddDays(1));
		}

		/// <summary>
		/// This test ensures that the ProductMapper can find the test product added since it will be within the time fram for updates
		/// </summary>
		[TestMethod]
		public void ProductMapper_GetMagentoProductsUpdatedAfter_WithValidUpdate()
		{
			var count = _productMapper.GetMagentoProductsUpdatedAfter(DateTime.Now.AddHours(-1)).Count();
			Assert.IsTrue(count > 0);
		}

		/// <summary>
		/// This test ensures that an invalid slug can't be added to endless aisle
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void ProductMapper_AddProductToEndlessAisle_AddInvalidSlugToEndlessAisle()
		{
			_productMapper.AddProductToEndlessAisle("X");
		}

		/// <summary>
		/// This test ensures that an empty slug can't be added to endless aisle
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void ProductMapper_AddProductToEndlessAisle_AddEmptySlugToEndlessAisle()
		{
			_productMapper.AddProductToEndlessAisle(string.Empty);
		}

		/// <summary>
		/// This test ensures that no catalog items are created when an invalid Document ID is supplied 
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void ProductMapper_AddProductHierarchyToEndlessAisle_InvalidDocumentId()
		{
			_productMapper.AddProductHierarchyToEndlessAisle(int.MaxValue);
		}

		/// <summary>
		/// If this test fails, the GetQuantityBySku function did not match the test value for quantity provided.
		/// </summary>
		[TestMethod]
		public void ProductMapper_GetQuantityBySku()
		{
			Assert.AreEqual(MagentoProductQuantity, _productMapper.GetQuantityBySku(MagentoProductSku));
		}
	}
}
