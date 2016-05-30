using System.Linq;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Tests.Utilities;

namespace Tests.ProductSync
{
    /// <summary>
    /// This test suite diagnoses possible problems with a Magento SKU, defined in TestHelper.cs
    /// </summary>
	[TestClass]
	public class ProductValid
	{
		private ProductResource _magentoProduct;

		[TestInitialize]
		public void SetUp()
		{
			_magentoProduct = TestHelper.TestProduct;
		}

        /// <summary>
        /// If this test fails, your Magento product sku may be invalid
        /// </summary>
		[TestMethod]
		public void MagentoProduct_Exists()
		{
			Assert.IsNotNull(_magentoProduct);
		}

        /// <summary>
        /// If this test fails, your product does not have an image
        /// </summary>
		[TestMethod]
		public void MagentoProduct_HasImages()
		{
			Assert.IsTrue(_magentoProduct.media_gallery_entries.Count > 0);
		}

        /// <summary>
        /// If this test fails, your product does not have a "base" image assigned
        /// </summary>
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

        /// <summary>
        /// If this test fails, your product does not have a category
        /// </summary>
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

        /// <summary>
        /// If this test fails, your product's category is not mapped in App.config
        /// </summary>
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

        /// <summary>
        /// If this test fails, your product does not have a manufacturer
        /// </summary>
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

        /// <summary>
        /// If this test fails, your product's manufacturer is not mapped in App.config
        /// </summary>
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
