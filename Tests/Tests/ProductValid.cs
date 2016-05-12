using System.Linq;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Tests.Utilities;

namespace Tests
{
	[TestClass]
	public class ProductValid
	{
		private ProductController _magentoProductController;
		private ProductResource _magentoProduct;

		[TestInitialize]
		public void SetUp()
		{
			var magentoAuthToken = App.GetMagentoAuthToken();
			_magentoProductController = new ProductController(magentoAuthToken);

			_magentoProduct = TestHelper.TestProduct;
		}

		//If this test fails, your Magento product sku may be invalid
		[TestMethod]
		public void MagentoProduct_Exists()
		{
			Assert.IsNotNull(_magentoProduct);
		}

		//If this test fails, your product does not have an image
		[TestMethod]
		public void MagentoProduct_HasImages()
		{
			Assert.IsTrue(_magentoProduct.media_gallery_entries.Count > 0);
		}

		//If this test fails, your product does not have a "base" image assigned
		[TestMethod]
		public void MagentoProduct_HasBaseImage()
		{
			object imageAttr = null;

			foreach (var option in _magentoProduct.custom_attributes.Where(option => option.attribute_code == ConfigReader.MagentoImageCode))
			{
				imageAttr = option.value;
			}

			Assert.IsNotNull(imageAttr);
			Assert.IsFalse(string.IsNullOrEmpty(imageAttr.ToString()));
		}

		//If this test fails, your product does not have a category
		[TestMethod]
		public void MagentoProduct_HasCategory()
		{
			object categoryAttr = null;

			foreach (var option in _magentoProduct.custom_attributes.Where(option => option.attribute_code == ConfigReader.MagentoCategoryCode))
			{
				categoryAttr = option.value;
			}

			Assert.IsNotNull(categoryAttr);
		}

		//If this test fails, your product's category is not mapped in App.config
		[TestMethod]
		public void MagentoProduct_HasMappedCategory()
		{
			JArray categoryAttr = null;
			var magentoCategoryId = -1;

			foreach (var option in _magentoProduct.custom_attributes.Where(option => option.attribute_code == ConfigReader.MagentoCategoryCode))
			{
				categoryAttr = (JArray) option.value;
			}

			Assert.IsNotNull(categoryAttr);

			magentoCategoryId = int.Parse(categoryAttr.First().ToString());

			Assert.IsTrue(ConfigReader.GetMatchingEndlessAisleCategory(magentoCategoryId) != -1);
		}

		//If this test fails, your product does not have a manufacturer
		[TestMethod]
		public void MagentoProduct_HasManufacturer()
		{
			object magentoAttr = null;

			foreach (var option in _magentoProduct.custom_attributes.Where(option => option.attribute_code == ConfigReader.MagentoManufacturerCode))
			{
				magentoAttr = option.value;
			}

			Assert.IsNotNull(magentoAttr);
		}

		//If this test fails, your product's manufacturer is not mapped in App.config
		[TestMethod]
		public void MagentoProduct_HasMappedManufacturer()
		{
			object manufacturerAttr = null;

			foreach (var option in _magentoProduct.custom_attributes.Where(option => option.attribute_code == ConfigReader.MagentoManufacturerCode))
			{
				manufacturerAttr = option.value;
			}

			Assert.IsNotNull(manufacturerAttr);

			var magentoManufacturerId = int.Parse(manufacturerAttr.ToString());

			Assert.IsTrue(ConfigReader.GetMatchingEndlessAisleManufacturer(magentoManufacturerId) != -1);
		}
	}
}
