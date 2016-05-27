using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.Magento
{
	[TestClass]
	public class CustomerControllerTests
	{
		//Private variables go here
		private CustomerController _customerController;
		private const int CustomerId = 1;

		[TestInitialize]
		public void SetUp()
		{
			string magentoAuthToken = App.GetMagentoAuthToken();
			_customerController = new CustomerController(magentoAuthToken);
		}

		//Tests go here

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the CustomerId provided is invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CustomerController_GetCustomer()
		{
			Assert.IsNotNull(_customerController.GetCustomer(CustomerId));
		}
	}
}
