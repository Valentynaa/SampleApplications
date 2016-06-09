using System;
using System.Collections.Generic;
using System.Linq;
using MagentoSync;
using MagentoSync.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.MockObjects.Controllers.EndlessAisle;
using Tests.MockObjects.Controllers.Magento;
using Tests.Utilities;

namespace Tests.Mappers
{
	/// <summary>
	/// This suite ensures the ProductMapper is working correctly
	/// 
	/// NOTE:
	///		This class does NOT use actual calls to the APIs and instead relies on mock controllers
	/// </summary>
	[TestClass]
	public class ProductMapperTests
	{
		private ProductMapper _productMapper;

		[TestInitialize]
		public void SetUp()
		{
			_productMapper = new ProductMapper(new MockCatalogsController(), new MockProductLibraryController(), new MockProductController());
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
		/// This test ensures that the ProductMapper can find the test product added since it will be within the time frame for updates
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
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ProductMapper_AddProductHierarchyToEndlessAisle_InvalidDocumentId()
		{
			_productMapper.AddProductHierarchyToEndlessAisle(int.MinValue);
		}
	}
}
