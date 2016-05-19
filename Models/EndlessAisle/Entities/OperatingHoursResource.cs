using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Entities
{
	[Serializable]
	public class OperatingHoursResource
	{
		public TimeResource Open { get; set; }

		public TimeResource Close { get; set; }
	}
}
