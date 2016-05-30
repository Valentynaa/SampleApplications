using MagentoConnect;
using MagentoConnect.Controllers.EndlessAisle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.EndlessAisle
{
    /// <summary>
    /// This suite ensures the EntitiesController is working correctly
    /// </summary>
	[TestClass]
	public class EntitiesControllerTests
	{
		private EntitiesController _entityController;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			_entityController = new EntitiesController(eaAuthToken);
		}

		/// <summary>
		/// This test ensures the request does not error
		/// </summary>
		[TestMethod]
		public void EntitiesController_GetLocation()
		{
			Assert.IsNotNull(_entityController.GetLocation());
		}

		/// <summary>
		/// This test ensures the request does not error
		/// </summary>
		[TestMethod]
		public void EntitiesController_GetAllManufacturers()
		{
			Assert.IsNotNull(_entityController.GetAllManufacturers());
		}
	}
}
