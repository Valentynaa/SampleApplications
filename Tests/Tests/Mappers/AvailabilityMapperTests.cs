﻿using System;
using System.Linq;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Mappers;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Tests.Mappers
{
	[TestClass]
	public class AvailabilityMapperTests
	{
		//Private variables go here
		private AvailabilityMapper _availabilityMapper;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_availabilityMapper = new AvailabilityMapper(magentoAuthToken, eaAuthToken);
		}

		//Tests go here

		/// <summary>
		/// This test ensures that an exception is thrown when null is passed in as the catalog
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void AvailabilityMapper_UpsertAvailabilityForCatalogItem_NullCatalogId()
		{
			_availabilityMapper.UpsertAvailabilityForCatalogItem(null);
		}

		/// <summary>
		/// This test ensures that an availability can be created from a new guid
		/// </summary>
		[TestMethod]
		public void AvailabilityMapper_UpsertAvailabilityForCatalogItem_NewGuid()
		{
			_availabilityMapper.UpsertAvailabilityForCatalogItem(Guid.NewGuid().ToString());
		}
	}
}