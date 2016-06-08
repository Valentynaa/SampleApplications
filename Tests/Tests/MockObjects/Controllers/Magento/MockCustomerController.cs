using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Customer;
using System.Collections.Generic;

namespace Tests.MockObjects.Controllers.Magento
{
	public class MockCustomerController : BaseMockController, ICustomerController
	{
		public CustomerResource GetCustomer(int customerId)
		{
			return new CustomerResource()
			{
				created_at = "2016-05-17 22:10:51",
				updated_at = "2016-05-17 22:10:51",
				id = customerId,
				group_id = 1,
				firstname = "Veronica",
				lastname = "Costello",
				addresses = new List<CustomerAddressResource>()
				{
					new CustomerAddressResource()
					{
						region = new CustomerRegionResource()
						{
							region_id = 33,
							region_code = "MI",
							region = "Michigan"
						},
						street = new List<string> { "123 Fake Street" },
						telephone = "5555555555",
						postcode = "H0H0H0",
						city = "Regina",
						firstname = "Joe",
						lastname = "Blow",
						default_billing = true,
						default_shipping = true
					}
				}
			};
		}
	}
}
