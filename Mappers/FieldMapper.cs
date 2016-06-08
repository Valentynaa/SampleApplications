using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MagentoConnect.Mappers
{
	public class FieldMapper : BaseMapper
	{
		private readonly IProductLibraryController _eaProductController;
		private readonly IProductController _magentoProductController;
		private readonly IFieldDefinitionController _eaFieldDefinitionController;
		private readonly ICustomAttributesController _magentoCustomAttributesController;

		public FieldMapper(IProductLibraryController productLibraryController, IProductController productController, IFieldDefinitionController fieldDefinitionController, ICustomAttributesController customAttributesController)
		{
			_eaProductController = productLibraryController;
			_magentoProductController = productController;
			_eaFieldDefinitionController = fieldDefinitionController;
			_magentoCustomAttributesController = customAttributesController;
		}

		/**
		 * Gets a matching Ea category given a magento category
		 * This will only get the FIRST category, as EA does not suppport multiple categories for a product
		 * 
		 * @param       magentoCustomAttributes     Magento custom attributes to search
		 *
		 * @return      int                         Id of a Classification or Category in EA that matches the Magento category for the Product
		 */
		public int GetMatchingCategory(List<CustomAttributeRefResource> magentoCustomAttributes)
		{
			//First get the list of Categories on the Magento product
			var magentoCategoryList =
				(JArray)GetAttributeByCode(magentoCustomAttributes, ConfigReader.MagentoCategoryCode);

			if (magentoCategoryList.Count == 0)
			{
				throw new Exception("No Magento cateogries found in the attributes list provided.");
			}

			//Next take the first one and convert it to an integer
			var magentoCategoryId = int.Parse(magentoCategoryList.FirstOrDefault().ToString());

			//Read from our mapping dictionary to get result
			var eaCategoryId = ConfigReader.GetMatchingEndlessAisleCategory(magentoCategoryId);

			if (eaCategoryId == -1)
			{
				throw new Exception(string.Format("Unable to find a matching category for Magento category {0}",
					magentoCategoryId));
			}

			return eaCategoryId;
		}

		/**
		 * Gets a matching Ea manufacturer given a magento manufacturer
		 * 
		 * @param       magentoCustomAttributes     Magento custom attributes to search
		 *
		 * @return      int                         Id of Manufacturer EA that matches the Magento category for the Product
		 */
		public int GetMatchingManufacturer(List<CustomAttributeRefResource> magentoCustomAttributes)
		{
			var manufacturerAttribute = GetAttributeByCode(magentoCustomAttributes, ConfigReader.MagentoManufacturerCode);
			if (manufacturerAttribute == null)
			{
				throw new Exception("No Magento manufacturers found in attribute list provided.");
			}

			//Get Identifier of Manufacturer
			var magentoManufacturerId =
				int.Parse(manufacturerAttribute.ToString());
			var eaManufacturerId = ConfigReader.GetMatchingEndlessAisleManufacturer(magentoManufacturerId);

			if (eaManufacturerId == -1)
			{
				throw new Exception(string.Format(
					"Unable to find a matching Manufacturer for Magento manufacturer {0}", magentoManufacturerId));
			}

			return eaManufacturerId;
		}

		/**
		 * This function will add a new Custom Property to a Magento product and store a SLUG in it
		 * This will serve to tie the two products together
		 * NOTE: The Magento product must use an attribute set that has an Endless Aisle Slug property added, 
		 * And you must ensure the MappingCode value at top is set to the attribute code
		 * 
		 * @param   magentoProduct  Magento product
		 * @param   slug            Slug of a Product in EA
		 *
		 */
		public void CreateMappingForProduct(ProductResource magentoProduct, string slug)
		{
			if (!Regex.IsMatch(slug, RegexPatterns.SlugPattern))
			{
				throw new Exception(string.Format("\"{0}\" is in an invalid slug format.", slug));
			}

			//Check for a mapping
			var mappingAttrValue = GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MappingCode);

			//mapping already exists? nothing to do!
			if (mappingAttrValue != null) return;

			//Get categories, required for updating product
			var magentoCategoryList = (JArray)GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MagentoCategoryCode);
			var categoryIds = magentoCategoryList.Select(category => int.Parse(category.ToString())).ToList();

			_magentoProductController.AddCustomAttributeToProduct(magentoProduct, categoryIds, ConfigReader.MappingCode, slug);
		}

		/**
		 * This function will return the product mapping for a magento product
		 * 
		 * @param   magentoProduct  Magento product
		 *
		 * @return  string          Product mapping string or null
		 */
		public string GetProductMapping(ProductResource magentoProduct)
		{
			var mapping = GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MappingCode);

			return mapping != null ? mapping.ToString() : null;
		}

		/**
		 * This function parses the custom properties in a magento product and creates a matching
		 * list of FieldResources for an EA product.
		 * If the product has a mapped value, it will check if any of the fields were DELETED and if so
		 * set the fields appropriately to have them deleted
		 * 
		 * @param   magentoProduct          Magento product to parse
		 *
		 * @return  List<FieldResource>     Matching field resources
		 */
		public List<FieldResource> ParseFieldsFromProduct(ProductResource magentoProduct)
		{
			var fields = new List<FieldResource>
				{
					CreateField(ConfigReader.GetValueForField(ConfigReader.MagentoNameCode),
								magentoProduct.name)
				};

			//Add Name field

			//Add fields from custom attributes
			foreach (var attribute in magentoProduct.custom_attributes)
			{
				FieldResource field = null;

				//Skip colors for now, as that requires a seperate API request after the MP is created
				if (attribute.attribute_code != ConfigReader.MagentoColorCode)
				{
					switch (
						_magentoCustomAttributesController.GetCustomAttributeIfExists(attribute.attribute_code)
							.frontend_input)
					{
						case "multiselect":
							field = CreateFieldFromMultiSelect(magentoProduct, attribute.attribute_code);
							break;
						case "text":
							field = CreateFieldFromText(magentoProduct, attribute.attribute_code);
							break;
						case "textarea":
							field = CreateFieldFromText(magentoProduct, attribute.attribute_code);
							break;
						case "select":
							field = CreateFieldFromSingleSelect(magentoProduct, attribute.attribute_code);
							break;
						default:
							Console.WriteLine(
									"Custom attribute {0} on product {1} (name) {2} (sku) was not synced as it is of a unsupported type. " +
									"Only multiselect, textarea and select are supported.",
									attribute.attribute_code, magentoProduct.name, magentoProduct.sku);
							break;
					}
				}

				if (field != null)
				{
					fields.Add(field);
				}
			}

			//If this is an "Upsert" 
			if (ProductHasMapping(magentoProduct))
			{
				var mappingSlug =
					GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MappingCode).ToString();

				var productDocumentId = GetProductDocumentIdFromSlug(mappingSlug);

				//Go through and ensure all product fields no longer on the product are "empty", this will ensure they are deleted
				var eaFields =
					_eaProductController.GetProductHierarchy(productDocumentId).RootRevision.FieldValues;

				foreach (var eaField in eaFields)
				{
					var deleted = true;
					//If matching field is missing from fields, add it but set it to null

					foreach (var field in fields)
					{
						if (eaField.FieldDefinitionId == field.FieldDefinitionId)
						{
							deleted = false;
						}
					}

					if (deleted)
					{
						fields.Add(new FieldResource
						{
							FieldDefinitionId = eaField.FieldDefinitionId,
							LanguageInvariantValue = ""
						});
					}
				}
			}

			return fields;
		}

		/**
		 * This function creates a FieldResource given a magento product and an attribute code for a TEXT field
		 * That means a field where you can only enter text
		 *
		 * @param   magentoProduct  Magento product to parse
		 * @param   attributeCode   Attribute code, hard coded value in Magento
		 *
		 * @return  FieldResource   Resource to add to EA product
		 */
		private static FieldResource CreateFieldFromText(ProductResource magentoProduct, string attrCode)
		{
			var value = GetAttributeByCode(magentoProduct.custom_attributes, attrCode);
			FieldResource field = null;

			if (value != null)
			{
				var fieldId = ConfigReader.GetValueForField(attrCode);

				if (fieldId != -1)
				{
					field = CreateField(ConfigReader.GetValueForField(attrCode), value.ToString());
				}
			}

			return field;
		}

		/**
		 * This function creates a FieldResource given a magento product and an attribute code for a SINGLE SELECT field
		 * That is a field where you can pick a SINGLE item from a drop down list
		 * 
		 * @param   magentoProduct  Magento product to parse
		 * @param   attributeCode   Attribute code, hard coded value in Magento
		 *
		 * @return  FieldResource   Resource to add to EA product
		 */
		private FieldResource CreateFieldFromSingleSelect(ProductResource magentoProduct, string attrCode)
		{
			var optionId = GetAttributeByCode(magentoProduct.custom_attributes, attrCode);
			FieldResource resource = null;
			var eaFieldId = ConfigReader.GetValueForField(attrCode);

			if (optionId != null && eaFieldId != -1)
			{
				var attribute = _magentoCustomAttributesController.GetCustomAttributeIfExists(attrCode);
				var value = GetLabelFromAttributeValue(attribute.options, optionId.ToString()).ToString();

				resource = new FieldResource { FieldDefinitionId = eaFieldId, LanguageInvariantValue = value };
			}

			return resource;
		}

		/**
		 * This function creates a FieldResource given a magento product and an attribute code for a MULTI SELECT field
		 * That means a field where you can pick multiple options from a dropdown
		 * NOTE:
		 * If this function runs into a property where the matching Ea field ALSO has an option list, they must have identical values
		 * Or the function will skip the option and continue on
		 * 
		 * @param   magentoProduct  Magento product to parse
		 * @param   attributeCode   Attribute code, hard coded value in Magento
		 *
		 * @return  FieldResource   Resource to add to EA product
		 */
		private FieldResource CreateFieldFromMultiSelect(ProductResource magentoProduct, string attrCode)
		{
			//Get option value from magento product
			var optionId = GetAttributeByCode(magentoProduct.custom_attributes, attrCode);
			FieldResource resource = null;

			//Get matching EA field identifier for this attribute
			var eaFieldId = ConfigReader.GetValueForField(attrCode);

			if (optionId != null && eaFieldId != -1)
			{
				//Get the specified option value, if there iso ne
				var attribute = _magentoCustomAttributesController.GetCustomAttributeIfExists(attrCode);
				var selectedValues = optionId.ToString().Split(',');

				var valueNames = new List<string>();

				//Get the Ea field definition
				var fieldDef = _eaFieldDefinitionController.GetAFieldDefinition(eaFieldId);

				foreach (var selectedValue in selectedValues)
				{
					//Determine if field definition has options, if so we want to cross reference them
					if (fieldDef.Options.Any())
					{
						//Get the Label for the selected option in magento from the custom attribute itself, where they are stored
						var valueName =
							GetLabelFromAttributeValue(attribute.options, selectedValue.ToString()).ToString();
						var value = fieldDef.Options.Find(x => x.Value == valueName);

						if (value != null)
						{
							valueNames.Add(value.Value);
						}
						else
						{
							Console.WriteLine(
									"Unable to add field from magento product {0} (name) {1} (sku) property {2} option {3} as there was no matching value in EA, continuing on",
									magentoProduct.name, magentoProduct.sku, attrCode, valueName);
						}
					}
					else
					{
						valueNames.Add(GetLabelFromAttributeValue(attribute.options, selectedValue).ToString());
					}
				}

				resource = new FieldResource
				{
					FieldDefinitionId = eaFieldId,
					LanguageInvariantValue = string.Join(", ", valueNames)
				};
			}

			return resource;
		}


		/**
		* This function creates a Field 
		* 
		* @param      fieldDefinitionId   Identifier of a FieldDefinition in EA    
		*
		* @returns    FieldResource       Object with reference to EA FieldDefinition for Name and magento product name
		*/
		private static FieldResource CreateField(int fieldDefinitionId, string name)
		{
			var nameField = new FieldResource
			{
				FieldDefinitionId = fieldDefinitionId,
				LanguageInvariantValue = name
			};

			return nameField;
		}
	}
}
