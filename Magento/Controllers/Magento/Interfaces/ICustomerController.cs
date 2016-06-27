using MagentoSync.Models.Magento.Customer;

namespace MagentoSync.Controllers.Magento.Interfaces
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