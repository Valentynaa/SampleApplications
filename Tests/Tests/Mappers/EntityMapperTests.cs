using MagentoConnect;
using MagentoConnect.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Mappers
{
    /// <summary>
    /// This suite ensures the EntityMapper is working correctly
    /// </summary>
	[TestClass]
	public class EntityMapperTests
	{
        //IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from your Magento system
        private const int RegionId = 23;

        private const string Country = "United States";
        private const string RegionCode = "IL";
        private const string City = "Chicago";

        private EntityMapper _entityMapper;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			var magentoAuthToken = App.GetMagentoAuthToken();
			_entityMapper = new EntityMapper(magentoAuthToken, eaAuthToken);
		}

		/// <summary>
		/// This test ensures that the correct data is returned on the first and successive calls to the EaLocation property
		/// </summary>
		[TestMethod]
		public void EntityMapper_EaLocation()
		{
			Assert.AreEqual(Country, _entityMapper.EaLocation.Address.CountryName);
			Assert.AreEqual(City, _entityMapper.EaLocation.Address.City);
		}

		/// <summary>
		/// This test ensures that the correct data is returned on the first and successive calls to the MagentoRegion property
		/// </summary>
		[TestMethod]
		public void EntityMapper_MagentoRegion()
		{
			Assert.AreEqual(RegionId, int.Parse(_entityMapper.MagentoRegion.id));
			Assert.AreEqual(RegionCode, _entityMapper.MagentoRegion.code);
		}
	}
}
