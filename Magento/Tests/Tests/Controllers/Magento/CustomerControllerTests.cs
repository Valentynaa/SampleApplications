using MagentoSync;
using MagentoSync.Controllers.Magento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.Magento
{
    /// <summary>
    /// This suite ensures the CustomerController is working correctly
    /// </summary>
    [TestClass]
	public class CustomerControllerTests
	{
        //IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from your Magento system
        private const int CustomerId = 1;

        private CustomerController _customerController;

		[TestInitialize]
		public void SetUp()
		{
			var magentoAuthToken = App.GetMagentoAuthToken();
			_customerController = new CustomerController(magentoAuthToken);
		}

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
