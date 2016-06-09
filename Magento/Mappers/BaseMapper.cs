using MagentoSync.Models.Magento.Products;
using MagentoSync.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MagentoSync.Controllers;

namespace MagentoSync.Mappers
{
	public class BaseMapper
	{
		/**
		 * Given a slug, parse out the product document Id
		 * 
		 * @param   slug    Generated identifier for a product in EA
		 *
		 * @return  int     Product document Id
		 */
		public int GetProductDocumentIdFromSlug(string slug)
		{
			if (slug.IndexOf("-", StringComparison.Ordinal) > -1)
			{
				slug = slug.Substring(0, slug.IndexOf("-", StringComparison.Ordinal));
			}

			return int.Parse(slug.Replace("M", ""));
		}

		/**
		 * This function will CHECK if a Product has a mapping in EA
		 *
		 * @param   magentoProduct  Magento product
		 *
		 * @return  bool    A flag to indicate if the Product has a mapping or not 
		 */
		public bool ProductHasMapping(ProductResource magentoProduct)
		{
			return GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MappingCode) != null;
		}

		/**
		 * This function returns the label associated with an attribute value
		 * 
		 * @param       resource    Custom attributes for a Magento product
		 * @param       value       Value of the option to retrieve
		 *
		 * @return      object      Label associated with the attribute option
		 */
		protected static object GetLabelFromAttributeValue(
			IEnumerable<Models.Magento.CustomAttributes.OptionResource> options, string value)
		{
			object label = null;

			//Get the option code
			foreach (var option in options.Where(option => value.ToLower() == option.value))
			{
				label = option.label;
			}

			if (label == null)
			{
				throw new Exception(string.Format("Product has no value for the option {0}!", value));
			}

			return label;
		}

		/**
		 * This function returns the value of a Magento custom attribute object given a code
		 * 
		 * @param       magentoCustomAttributes     Custom attributes for a Magento product
		 * @param       attributeCode               Attribute code, hard coded value in Magento
		 *
		 * @return      object                      Value inside custom attribute
		 */
		public static object GetAttributeByCode(List<CustomAttributeRefResource> magentoCustomAttributes,
			string attributeCode)
		{
			object value = null;

			//Get the option code
			foreach (var attr in magentoCustomAttributes.Where(attr => attr.attribute_code == attributeCode))
			{
				value = attr.value;
			}

			return value;
		}
	}
}
