using System;
using System.Linq;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Mappers;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Tests.Utilities;

namespace Tests.Mappers
{
	[TestClass]
	public class BaseMapperTests
	{
		//Private variables go here
		private BaseMapper _baseMapper;
		private ProductResource _magentoTestProduct;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_baseMapper = new BaseMapper(magentoAuthToken, eaAuthToken);
			_magentoTestProduct = TestHelper.TestProduct;
		}

		//Tests go here

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
