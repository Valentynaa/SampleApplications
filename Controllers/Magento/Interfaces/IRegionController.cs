using System.Collections.Generic;
using MagentoConnect.Models.Magento.Country;

namespace MagentoConnect.Controllers.Magento
{
	public interface IRegionController : IController
	{
		/// <summary>
		/// Get the coutries and their region data from Magento
		/// </summary>
		/// <returns>List of coutries and their region data</returns>
		IEnumerable<CountryResource> GetCountries();
	}
}