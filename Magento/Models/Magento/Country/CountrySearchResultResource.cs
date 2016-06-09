using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoSync.Models.Magento.Country
{
	[Serializable]
	public class CountrySearchResultResource
	{
		public IEnumerable<CountryResource> Items { get; set; }
	}
}
