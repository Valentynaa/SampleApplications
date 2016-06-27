using MagentoSync.Controllers.Magento.Interfaces;
using MagentoSync.Models.Magento.Customer;
using MagentoSync.Utilities;

namespace MagentoSync.Mappers
{
	public class CustomerMapper : BaseMapper
	{
		private readonly ICustomerController _magentoCustomerController;
		public CustomerMapper(ICustomerController customerController)
		{
			_magentoCustomerController = customerController;
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
