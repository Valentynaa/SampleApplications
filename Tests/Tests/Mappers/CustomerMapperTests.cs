using MagentoConnect;
using MagentoConnect.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Mappers
{
	/// <summary>
	/// This suite ensures the CustomerMapper is working correctly
	/// </summary>
	[TestClass]
	public class CustomerMapperTests
	{
		//IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from your Magento system
		private const string FirstName = "Joe";
		private const int CustomerId = 2;

		private CustomerMapper _customerMapper;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			var magentoAuthToken = App.GetMagentoAuthToken();
			_customerMapper = new CustomerMapper(magentoAuthToken, eaAuthToken);
		}

		/// <summary>
		/// This test ensures that the correct data is returned on the first and successive calls to the MagentoCustomer property
		/// </summary>
		[TestMethod]
		public void CustomerMapper_GetCustomer()
		{
			Assert.AreEqual(FirstName, _customerMapper.MagentoCustomer.firstname);
			Assert.AreEqual(CustomerId, _customerMapper.MagentoCustomer.id);
		}
	}
}
