using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Entities
{
	[Serializable]
	public class AddressResource
	{
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string StateCode { get; set; }
		public string StateName { get; set; }
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public string Zip { get; set; }
	}
}
