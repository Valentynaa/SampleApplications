using System;
using System.Globalization;
using System.Linq;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Models.Magento.CustomAttributes;
using System.Collections.Generic;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;

namespace MagentoConnect.Mappers
{
	public class ColorMapper : BaseMapper
	{
		private readonly CustomAttributeResource _customAttributeColor;

		private readonly ICustomAttributesController _magentoCustomAttributesController;
		private readonly IProductLibraryController _eaProductController;

		public ColorMapper(ICustomAttributesController customAttributesController, IProductLibraryController productLibraryController)
		{
			_magentoCustomAttributesController = customAttributesController;
			_eaProductController = productLibraryController;

			_customAttributeColor = _magentoCustomAttributesController.GetCustomAttributeIfExists(ConfigReader.MagentoColorCode);
		}

		/**
		 * Creates a color definition for a product in EA
		 * LIMITATION: Cannot remove color definitions
		 * 
		 * @param   productDocumentId       Identifier of product in EA to add color definition to
		 * @param   magentoColorId          Magento color option Id
		 *
		 * @return  string                  Identifier of created Color Definition
		 */
		public string UpsertColorDefinitions(int productDocumentId, int magentoColorId)
		{
			var colorTag = ConfigReader.GetMatchingEndlessAisleColor(magentoColorId);

			if (colorTag == -1)
				return null;

			var colorTags = new List<int> { colorTag };
			var colorName =
				GetLabelFromAttributeValue(_customAttributeColor.options, magentoColorId.ToString(CultureInfo.InvariantCulture)).ToString();
			var existingColorDefinitions = _eaProductController.GetColorDefinitions(productDocumentId);

			if (existingColorDefinitions != null)
			{
				//Check if the color is already defined - only compare name and color tags
				foreach (var existingColorDef in existingColorDefinitions.ColorDefinitions)
				{
					if (colorName == existingColorDef.Name &&
						Equals(colorTags, existingColorDef.ColorTagIds))
					{
						return existingColorDef.Id.ToString();
					}
				}
			}

			//Create color definition
			return CreateColorDefinition(productDocumentId,
				new ColorDefinitionResource
				{
					ColorTagIds = colorTags,
					Name = colorName,
					Swatch = null
				}).Id.ToString();        
		}

		/**
		 * This function creates color definitions for a product in EA, which allows a swatch to appear for selecting colors
		 * Swatch is not supported
		 *
		 * @param   colorDef                    Object representing color definition to add
		 *
		 * @return  ColorDefinitionResource     Created color definition
		 */
		public ColorDefinitionResource CreateColorDefinition(int productDocumentId, ColorDefinitionResource colorDef)
		{
			if (productDocumentId < 1)
				throw new ArgumentOutOfRangeException(nameof(productDocumentId));

			var colorDefList = new List<ColorDefinitionResource> {colorDef};

			//Create color definition
			var results = _eaProductController.CreateColorDefinition(productDocumentId, new ColorDefinitionsResource
			{
				ColorDefinitions = colorDefList
			});

			if (colorDef != null)
			{
				//Return the right one (irritating workarond)
				foreach (var def in results.ColorDefinitions)
				{
					if (def.Name == colorDef.Name)
					{
						colorDef = def;
					}
				}
			}

			return colorDef;
		}

		/**
		 * Gets a magento color attribute given a product, if the color is defined
		 * 
		 * @param   magentoProduct          Magento Product to parse
		 *
		 * @return  string                  Color identifier as a string
		 */
		public string GetMagentoColorAttribute(ProductResource magentoProduct)
		{
			if (_customAttributeColor == null || magentoProduct == null)
				return null;

			var colorObj = GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MagentoColorCode);

			return colorObj == null ? null : colorObj.ToString();
		}
	}
}
