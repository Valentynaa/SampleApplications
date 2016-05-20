using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Country;
using MagentoConnect.Models.Magento.Customer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.Magento
{
	[TestClass]
	public class RegionControllerTests
	{
		//Private variables go here
		private RegionController _regionController;

		[TestInitialize]
		public void SetUp()
		{
			string magentoAuthToken = App.GetMagentoAuthToken();
			_regionController = new RegionController(magentoAuthToken);
		}

		//Tests go here

		/// <summary>
		/// This test ensures the request does not error.
		/// </summary>
		[TestMethod]
		public void RegionController_GetCountries()
		{
			IEnumerable<CountryResource> countries = _regionController.GetCountries();
		}
	}
}
