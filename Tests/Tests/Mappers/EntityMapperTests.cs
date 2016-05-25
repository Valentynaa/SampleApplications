using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect;
using MagentoConnect.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Mappers
{
	[TestClass]
	public class EntityMapperTests
	{
		//Private variables go here
		private EntityMapper _entityMapper;
		private const string Country = "United States";
		private const string RegionCode = "IL";
		private const string City = "Chicago";
		private const int RegionId = 23;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_entityMapper = new EntityMapper(magentoAuthToken, eaAuthToken);
		}

		//Tests go here

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
