using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.Magento
{
    /// <summary>
    /// This suite ensures the RegionController is working correctly
    /// </summary>
    [TestClass]
	public class RegionControllerTests
	{
		private RegionController _regionController;

		[TestInitialize]
		public void SetUp()
		{
			var magentoAuthToken = App.GetMagentoAuthToken();
			_regionController = new RegionController(magentoAuthToken);
		}

		/// <summary>
		/// This test ensures the request does not error.
		/// </summary>
		[TestMethod]
		public void RegionController_GetCountries()
		{
			Assert.IsNotNull(_regionController.GetCountries());
		}
	}
}
