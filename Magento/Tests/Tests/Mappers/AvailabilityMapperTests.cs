using System;
using MagentoSync;
using MagentoSync.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.MockObjects.Controllers.EndlessAisle;

namespace Tests.Mappers
{
	/// <summary>
	/// This suite diagnoses problems with the Availability sync
	/// 
	/// NOTE:
	///		This class does NOT use actual calls to the APIs and instead relies on mock controllers
	/// </summary>
	[TestClass]
	public class AvailabilityMapperTests
	{
		private AvailabilityMapper _availabilityMapper;

		[TestInitialize]
		public void SetUp()
		{
			_availabilityMapper = new AvailabilityMapper(new MockAvailabilityController());
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
