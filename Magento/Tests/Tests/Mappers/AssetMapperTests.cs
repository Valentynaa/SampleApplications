using System;
using MagentoSync;
using MagentoSync.Mappers;
using MagentoSync.Models.Magento.Products;
using MagentoSync.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.MockObjects.Controllers.EndlessAisle;
using Tests.Utilities;

namespace Tests.Mappers
{
	/// <summary>
	/// This suite diagnoses problems with adding images to a product
	/// 
	/// NOTE:
	///		This class does NOT use actual calls to the APIs and instead relies on mock controllers
	/// </summary>
	[TestClass]
	public class AssetMapperTests
	{
		private AssetMapper _assetMapper;
		private ProductResource _magentoTestProduct;

		[TestInitialize]
		public void SetUp()
		{
			_assetMapper = new AssetMapper(new MockAssetsController(), new MockProductLibraryController());
			_magentoTestProduct = TestHelper.MockTestProduct;
		}

		/// <summary>
		/// If this test fails, the the GetHeroShot function is not behaving as intended or the
		/// test product has no image attached to it in magento.
		/// </summary>
		[TestMethod]
		public void AssetMapper_GetHeroShot()
		{
			Assert.IsNotNull(_assetMapper.GetHeroShot(_magentoTestProduct));
		}

		/// <summary>
		/// If this test fails, your product does not have an image or the ParseAssetsFromProduct function 
		/// is unable to find the product assets
		/// </summary>
		[TestMethod]
		public void AssetMapper()
		{
			Assert.IsNotNull(_assetMapper.ParseAssetsFromProduct(_magentoTestProduct, ConfigReader.MagentoStorageConfiguration));
		}
	}
}
