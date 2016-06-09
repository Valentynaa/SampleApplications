using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoSync.Models.EndlessAisle.Entities
{
	[Serializable]
	public class StoreHoursResource
	{
		public OperatingHoursResource Monday { get; set; }

		public OperatingHoursResource Tuesday { get; set; }

		public OperatingHoursResource Wednesday { get; set; }

		public OperatingHoursResource Thursday { get; set; }

		public OperatingHoursResource Friday { get; set; }

		public OperatingHoursResource Saturday { get; set; }

		public OperatingHoursResource Sunday { get; set; }
	}
}
