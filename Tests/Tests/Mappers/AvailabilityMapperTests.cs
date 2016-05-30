using System;
using MagentoConnect;
using MagentoConnect.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Mappers
{
    /// <summary>
    /// This suite diagnoses problems with the Availability sync
    /// </summary>
	[TestClass]
	public class AvailabilityMapperTests
	{
		private AvailabilityMapper _availabilityMapper;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			var magentoAuthToken = App.GetMagentoAuthToken();
			_availabilityMapper = new AvailabilityMapper(magentoAuthToken, eaAuthToken);
		}

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
