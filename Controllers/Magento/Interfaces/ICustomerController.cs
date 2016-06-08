using MagentoConnect.Models.Magento.Customer;

namespace MagentoConnect.Controllers.Magento
{
	public interface ICustomerController : IController
	{
		/// <summary>
		/// Gets the Magento Customer specified
		/// </summary>
		/// <param name="customerId">Customer identifier</param>
		/// <returns>Magento Customer</returns>
		CustomerResource GetCustomer(int customerId);
	}
}