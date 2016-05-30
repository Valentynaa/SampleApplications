using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Customer;
using MagentoConnect.Utilities;

namespace MagentoConnect.Mappers
{
	public class CustomerMapper : BaseMapper
	{
		private readonly CustomerController _magentoCustomerController;
		public CustomerMapper(string magentoAuthToken, string eaAuthToken) : base(magentoAuthToken, eaAuthToken)
		{
			_magentoCustomerController = new CustomerController(magentoAuthToken);
		}

		//Magento customer from customer ID in App.config
		public CustomerResource MagentoCustomer
		{
			get
			{
				if (_magentoCustomer == null)
				{
					_magentoCustomer = _magentoCustomerController.GetCustomer(ConfigReader.MagentoCustomerId);
				}
				return _magentoCustomer;
			}
		}
		private CustomerResource _magentoCustomer;
	}
}
