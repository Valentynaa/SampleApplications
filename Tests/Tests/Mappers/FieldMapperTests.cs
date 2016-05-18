﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
	public class FieldMapperTests
	{
		//Private variables go here
		//Configure test variables to match a product in your Magento system
		private const int EAManufacturerId = 9829;
		private FieldMapper _fieldMapper;
		private ProductResource _magentoTestProduct;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_fieldMapper = new FieldMapper(magentoAuthToken, eaAuthToken);
			_magentoTestProduct = TestHelper.TestProduct;
		}

		//Tests go here

		/// <summary>
		/// The test ensures that trying to match to an invalid magento category will throw an exception.
		/// This is done by trying to find a category with the value of -1
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void FieldMapper_GetMatchingCategory_GetInvalidEntry()
		{
			List<CustomAttributeRefResource> attributes = new List<CustomAttributeRefResource>
			{
				new CustomAttributeRefResource { attribute_code = ConfigReader.MagentoCategoryCode, value = new JArray { -1 } }
			};
			_fieldMapper.GetMatchingCategory(attributes);
		}

		/// <summary>
		/// The test ensures that trying to match to an empty list of categories will throw an exception.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void FieldMapper_GetMatchingCategory_GetEmptyCategory()
		{
			List<CustomAttributeRefResource> attributes = new List<CustomAttributeRefResource>
			{
				new CustomAttributeRefResource { attribute_code = ConfigReader.MagentoCategoryCode, value = new JArray() }
			};
			_fieldMapper.GetMatchingCategory(attributes);
		}

		/// <summary>
		/// If this test fails, either your test Manufacturer ID or the GetMatchingManufacturer function is incorrect.
		/// You can check your Manufacturer IDs by running the Getting All Manufacturers request in Postman.
		/// </summary>
		[TestMethod]
		public void FieldMapper_GetMatchingManufacturer_VerfiyManufacturer()
		{
			Assert.IsTrue(_fieldMapper.GetMatchingManufacturer(_magentoTestProduct.custom_attributes) == EAManufacturerId);
		}

		/// <summary>
		/// This test ensures that an exception is thrown when no manufacturer code is provided.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void FieldMapper_GetMatchingManufacturer_GetEmptyManufacturer()
		{
			_magentoTestProduct.custom_attributes.RemoveAll(x => x.attribute_code == ConfigReader.MagentoManufacturerCode);
			_fieldMapper.GetMatchingManufacturer(_magentoTestProduct.custom_attributes);
		}

		/// <summary>
		/// This test ensures that an exception is thrown in the event that an invalid Slug is provided
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void FieldMapper_CreateMappingForProduct_InvalidSlug()
		{
			_fieldMapper.CreateMappingForProduct(_magentoTestProduct, "X");
		}

		/// <summary>
		/// If this test fails then either the fields were not parsed from the product or extra fields were
		/// parsed that should not be there. 
		/// </summary>
		[TestMethod]
		public void FieldMapper_ParseFieldsFromProduct_VerifyFieldsReturned()
		{
			var fields =_fieldMapper.ParseFieldsFromProduct(_magentoTestProduct);
			var magentoCustomAttributes = _magentoTestProduct.custom_attributes;
			Assert.IsTrue(fields.Count <= magentoCustomAttributes.Count && fields.Count != 0);
		}
	}
}