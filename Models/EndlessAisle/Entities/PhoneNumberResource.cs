using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Entities
{
	[Serializable]
	public class PhoneNumberResource
	{
		public string Description { get; set; }
		public string Number { get; set; }
		public string Extension { get; set; }
	}
}
