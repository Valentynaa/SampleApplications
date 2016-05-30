using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Entities
{
	[Serializable]
	public class ContactResource
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<PhoneNumberResource> PhoneNumbers { get; set; }
	}
}
