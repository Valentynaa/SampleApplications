using System;
using System.Collections.Generic;
using MagentoConnect;
using MagentoConnect.Mappers;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Models.Magento.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.MockObjects.Controllers.EndlessAisle;
using Tests.MockObjects.Controllers.Magento;
using Tests.Utilities;

namespace Tests.Mappers
{
	/// <summary>
	/// This suite ensures the ColorMapper is working correctly
	/// 
	/// NOTE:
	///		This class does NOT use actual calls to the APIs and instead relies on mock controllers
	/// </summary>
	[TestClass]
	public class ColorMapperTests
	{
		//This value only needs to be greater than 0
		private const int EaProductDocumentId = 1;

		private readonly int _colorId = MockCustomAttributesController.MappedColorId;
		private ColorMapper _colorMapper;
		private ProductResource _magentoTestProduct;

		[TestInitialize]
		public void SetUp()
		{
			_colorMapper = new ColorMapper(new MockCustomAttributesController(), new MockProductLibraryController());
			_magentoTestProduct = TestHelper.MockTestProduct;
		}

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
			Assert.IsNotNull(_colorMapper.UpsertColorDefinitions(EaProductDocumentId, _colorId));
		}

		/// <summary>
		/// This test ensures that a color definition is not created when
		/// an invalid product document ID is supplied.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ColorMapper_CreateColorDefinition_InvalidProductDocumentId()
		{
			_colorMapper.CreateColorDefinition(int.MinValue,
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
