using System;
using MagentoConnect;
using MagentoConnect.Mappers;
using MagentoConnect.Models.Magento.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Mappers
{
	/// <summary>
	/// This suite diagnoses problems with mapping values
	/// 
	/// NOTE:
	///		This class does NOT use actual calls to the APIs and instead relies on mock controllers
	/// </summary>
	[TestClass]
	public class BaseMapperTests
	{
		private BaseMapper _baseMapper;
		private ProductResource _magentoTestProduct;

		[TestInitialize]
		public void SetUp()
		{
			_baseMapper = new BaseMapper();
			_magentoTestProduct = TestHelper.MockTestProduct;
		}

		/// <summary>
		/// If this test fails, your product does not have a mapping code
		/// </summary>
		[TestMethod]
		public void BaseMapper_ProductHasMapping()
		{
			Assert.IsTrue(_baseMapper.ProductHasMapping(_magentoTestProduct));
		}

		/// <summary>
		/// This test ensures that the BaseMapper can get the document ID from the slug provided correctly.
		/// </summary>
		[TestMethod]
		public void BaseMapper_GetProductDocumentIdFromSlug()
		{
			Assert.IsTrue(_baseMapper.GetProductDocumentIdFromSlug("M2039") == 2039);
			Assert.IsTrue(_baseMapper.GetProductDocumentIdFromSlug("M2039-V3") == 2039);
			try
			{
				//Invalid slug
				_baseMapper.GetProductDocumentIdFromSlug("XX-XX");
			}
			catch (Exception)
			{
				return;
			}
			Assert.Fail("No exception was thrown.");
		}
	}
}
