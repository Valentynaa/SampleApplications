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
	public class CustomerMapperTests
	{
		//Private variables go here
		private CustomerMapper _customerMapper;
		private const string FirstName = "Joe";
		private const int CustomerId = 2;

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_customerMapper = new CustomerMapper(magentoAuthToken, eaAuthToken);
		}

		//Tests go here

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
