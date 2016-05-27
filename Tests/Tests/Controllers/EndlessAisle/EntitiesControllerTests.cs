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
	public class EntitiesControllerTests
	{
		//Private variables go here
		private EntitiesController _entityController;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			_entityController = new EntitiesController(eaAuthToken);
		}

		//Tests go here

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
