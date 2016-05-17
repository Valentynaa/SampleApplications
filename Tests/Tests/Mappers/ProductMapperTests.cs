using System;
using System.Collections.Generic;
using System.Linq;
using MagentoConnect;
using MagentoConnect.Controllers;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using MagentoConnect.Mappers;
using MagentoConnect.Models.Magento.CustomAttributes;
using RestSharp;
using Tests.Utilities;

namespace Tests.Mappers
{
	[TestClass]
	public class ProductMapperTests
	{
		//Private variables go here
		private ProductMapper _productMapper;
		private const int MagentoProductId = 1;
		private const string MagentoProductSku = "24-MB01";
		private const int MagentoCategoryId = 4;
		private const int MagentoProductQuantity = 100;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_productMapper = new ProductMapper(magentoAuthToken, eaAuthToken);
			TestHelper.CreateTestUpdate(magentoAuthToken, MagentoProductId, MagentoProductSku, new List<int> { MagentoCategoryId });
		}

		//Tests go here

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
			int count = _productMapper.GetMagentoProductsUpdatedAfter(DateTime.Now.AddHours(-1)).Count();
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
			_productMapper.AddProductToEndlessAisle(String.Empty);
		}

		/// <summary>
		/// This test ensures that no catalog items are created when an invalid Document ID is supplied 
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void ProductMapper_AddProductHierarchyToEndlessAisle_InvalidDocumentId()
		{
			_productMapper.AddProductHierarchyToEndlessAisle(Int32.MaxValue);
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
