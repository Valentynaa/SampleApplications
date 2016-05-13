using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect;
using MagentoConnect.Controllers.EndlessAisle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.EndlessAisle
{
	[TestClass]
	public class CatalogsControllerTests
	{
		//Private variables go here
		private CatalogsController _catalogsController;
		private const string Slug = "M2039";

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			_catalogsController = new CatalogsController(eaAuthToken);
		}

		//Tests go here

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
