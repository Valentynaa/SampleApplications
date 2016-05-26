using System;
using System.Collections.Generic;
using System.Linq;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Mappers;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Tests.Utilities;

namespace Tests.Mappers
{
	[TestClass]
	public class ColorMapperTests
	{
		//Private variables go here
		private ColorMapper _colorMapper;
		private ProductResource _magentoTestProduct;
		private const int EaProductDocumentId = 2039;
		private const int MappedColorId = 49;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_colorMapper = new ColorMapper(magentoAuthToken, eaAuthToken);
			_magentoTestProduct = TestHelper.TestProduct;
		}

		//Tests go here

		/// <summary>
		/// This test ensures that the UpsertColorDefinitions function returns null when the color ID is not mapped
		/// and that correct color IDs can be upserted.
		/// 
		/// Check App.config for mappings
		/// </summary>
		[TestMethod]
		public void ColorMapper_UpsertColorDefinitions()
		{
			//Color ID 0 is not mapped and color ID 1 is black
			Assert.IsNull(_colorMapper.UpsertColorDefinitions(EaProductDocumentId, 0));
			Assert.IsNotNull(_colorMapper.UpsertColorDefinitions(EaProductDocumentId, MappedColorId));
		}

		/// <summary>
		/// This test ensures that a color definition is not created when
		/// an invalid product document ID is supplied.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void ColorMapper_CreateColorDefinition_InvalidProductDocumentId()
		{
			_colorMapper.CreateColorDefinition(Int32.MaxValue,
				 new ColorDefinitionResource
				 {
					 ColorTagIds = new List<int>(),
					 Name = "Transparent",
					 Swatch = null
				 });
		}

		/// <summary>
		/// If this test fails, the the GetMagentoColorAttribute is not behaving as intended or the
		/// test product has no color attribute.
		/// </summary>
		[TestMethod]
		public void ColorMapper_GetMagentoColorAttribute()
		{
			Assert.IsNull(_colorMapper.GetMagentoColorAttribute(null));
			Assert.IsNotNull(_colorMapper.GetMagentoColorAttribute(_magentoTestProduct));
		}
	}
}
