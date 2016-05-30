using System;
using MagentoConnect;
using MagentoConnect.Mappers;
using MagentoConnect.Models.Magento.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Mappers
{
    /// <summary>
    /// This suite diagnoses problems with adding images to a product
    /// </summary>
	[TestClass]
	public class AssetMapperTests
	{
		private AssetMapper _assetMapper;
		private ProductResource _magentoTestProduct;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			var magentoAuthToken = App.GetMagentoAuthToken();
			_assetMapper = new AssetMapper(magentoAuthToken, eaAuthToken);
			_magentoTestProduct = TestHelper.TestProduct;
		}

		/// <summary>
		/// If this test fails, the the GetHeroShot function is not behaving as intended or the
		/// test product has no image attached to it in magento.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void AssetMapper_GetHeroShot()
		{
			Assert.IsNotNull(_assetMapper.GetHeroShot(_magentoTestProduct));
			_assetMapper.GetHeroShot(null);
		}

		/// <summary>
		/// If this test fails, your product does not have an image or the ParseAssetsFromProduct function 
		/// is unable to find the product assets
		/// </summary>
		[TestMethod]
		public void AssetMapper()
		{
			Assert.IsNotNull(_assetMapper.ParseAssetsFromProduct(_magentoTestProduct));
		}
	}
}
