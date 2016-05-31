using System;
using System.Linq;
using MagentoConnect;
using MagentoConnect.Controllers.EndlessAisle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.EndlessAisle
{
	/// <summary>
	/// This suite ensures the CatalogsController is working correctly
	/// </summary>
	[TestClass]
	public class CatalogsControllerTests
	{
		//IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from Endless Aisle
		private const string Slug = "M2039";

		private CatalogsController _catalogsController;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			_catalogsController = new CatalogsController(eaAuthToken);
		}

		/// <summary>
		/// If this test fails, the GetCatalogItemsBySlug did not return any items.
		/// Uses Slug provided to find items.
		/// </summary>
		[TestMethod]
		public void CatalogsController_GetCatalogItemsBySlug()
		{
			Assert.IsNotNull(_catalogsController.GetCatalogItemsBySlug(Slug).FirstOrDefault());
		}

		/// <summary>
		/// This test ensures an error is thrown if catalog items are requested for an invalid slug.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void CatalogsController_GetCatalogItemsBySlug_InvalidSlug()
		{
			Assert.IsNotNull(_catalogsController.GetCatalogItemsBySlug("XX").FirstOrDefault());
		}
	}
}
